# Shared Kernel — دليل الاستخدام للفريق

> **اللي نزل:** Result Pattern + جسر الـ HTTP + Pagination + BaseEntity الجديدة — كلها في `feature/catalog-grades` (PR قيد المراجعة).
> **القاعدة:** أي Feature جديدة تبدأها، استخدم الـ Result بدل ما ترمي `AppException` في الحالات المتوقعة (Not Found / Conflict / Forbidden). الـ Exceptions فضلت للحالات غير المتوقعة والـ ValidationBehavior.

---

## 1) Result Pattern في الـ Handler

**Query بترجع قيمة:**

```csharp
public sealed class GetSubjectByIdQueryHandler(IApplicationDbContext context)
    : IRequestHandler<GetSubjectByIdQuery, Result<SubjectResponse>>
{
    public async Task<Result<SubjectResponse>> Handle(GetSubjectByIdQuery request, CancellationToken cancellationToken)
    {
        var subject = await context.Subjects
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken);

        if (subject is null)
            return Result.Failure<SubjectResponse>(Error.NotFound("Subject", request.Id));

        return Result.Success(new SubjectResponse(subject.Id, subject.Name));
    }
}
```

**Command من غير قيمة:**

```csharp
public async Task<Result> Handle(DeleteYearCommand request, CancellationToken cancellationToken)
{
    var year = await context.Years.FirstOrDefaultAsync(y => y.Id == request.Id, cancellationToken);
    if (year is null)
        return Result.Failure(Error.NotFound("Year", request.Id));

    context.Years.Remove(year);
    await context.SaveChangesAsync(cancellationToken);
    return Result.Success();
}
```

**أخطاء تانية جاهزة:**

```csharp
Result.Failure(Error.Conflict("YEAR_IN_USE", "Cannot delete a year that has subjects."));
Result.Failure(Error.Forbidden("NOT_OWNER", "You can only edit your own notes."));
Result.Failure(Error.Failure("EXPORT_FAILED", "Could not generate the report."));
```

**أخطاء حقول (Validation) من جوه الـ business logic:**

```csharp
return Result.ValidationFailure(new Dictionary<string, string[]>
{
    ["score"] = ["Score cannot exceed the max score."]
});
// → 400 / code: VALIDATION_ERROR / errors: { "score": [...] } — نفس شكل الـ ValidationException بالظبط
```

> الـ FluentValidation في مجلد `Validations` لسه شغال زي ما هو وبيشتغل **قبل** الـ Handler تلقائياً.

---

## 2) الـ Controller — سطر واحد

```csharp
using EduNova.API.Common;   // ← متنساش الـ using

[HttpGet("{id:guid}")]
public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
    => (await _mediator.Send(new GetSubjectByIdQuery(id), cancellationToken)).ToActionResult(this);

[HttpDelete("{id:guid}")]
public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken cancellationToken)
    => (await _mediator.Send(new DeleteYearCommand(id), cancellationToken)).ToActionResult(this);
```

| النتيجة | الـ HTTP |
|---|---|
| `Result<T>` نجاح | **200** + القيمة |
| `Result` نجاح | **204** No Content |
| أي فشل | **نفس ProblemDetails القديم بالظبط** (title / detail / code / traceId / errors) |

الفشل متأكد إنه متطابق مع مسار الـ Exceptions — الاتنين بيعبروا على نفس الـ builder (`ApiProblem`)، فالـ Frontend مش هيحس بفرق.

---

## 3) Pagination

**الـ Query بتورث `PagedRequest`** — عشان `?page=2&pageSize=20` يتبني مسطح:

```csharp
public class GetSubjectsQuery : PagedRequest, IRequest<Result<PagedResult<SubjectListItemResponse>>>
{
    public string? Search { get; set; }
    public Guid? YearId { get; set; }
}
```

**الـ Validator بيلحق قواعد الـ paging** (FluentValidation مش بيطبقها تلقائياً — لازم Include):

```csharp
public class GetSubjectsQueryValidator : AbstractValidator<GetSubjectsQuery>
{
    public GetSubjectsQueryValidator()
    {
        Include(new PagedRequestValidator());   // Page >= 1 و PageSize 1..50
        RuleFor(x => x.Search).MaximumLength(100);
    }
}
```

**الـ Handler — الفلترة والترتيب قبل الـ paging:**

```csharp
var query = context.Subjects.AsNoTracking();

if (!string.IsNullOrWhiteSpace(request.Search))
    query = query.Where(s => s.Name.Contains(request.Search) || s.Code.Contains(request.Search));
if (request.YearId is not null)
    query = query.Where(s => s.YearId == request.YearId);

query = query.OrderBy(s => s.Name);

var page = await query.ToPagedResultAsync(request, cancellationToken);
return Result.Success(page);
```

> ⚠️ الفلترة والترتيب **قبل** `ToPagedResultAsync` — عشان الكونت والصفحة يتعدوا على الـ SQL Server مش في الذاكرة. الـ `MaxPageSize` = 50.

**شكل الـ response:**

```json
{
  "items": [ ... ],
  "page": 1, "pageSize": 10, "totalCount": 25,
  "totalPages": 3, "hasNext": true, "hasPrevious": false
}
```

---

## 4) Specifications — بتوصف "إيه" من غير "إزاي"

الـ Specification بتوصف **إيه** الداتا المطلوبة (فلترة / بحث / includes / ترتيب) — **ولا حرف منها** عن الـ paging. الـ Skip/Take فضلوا حصراً في `ToPagedResultAsync`.

**الـ flow الثابت:**
```
Request → Specification → IQueryable → ToPagedResultAsync(PagedRequest) → PagedResult
```

**الأساس في `Application/Common/Specifications/`:** `ISpecification<T>` + `Specification<T>` بـ:
- `Where(expr)` — الشروط (بتتركب بـ AND)
- `OrderBy(expr, desc)` / `ThenBy(expr, desc)` — أول نداء OrderBy والباقي ThenBy
- `Include(expr)` — أول مستوى navigation (ومش محتاجه لو هتعمل `Select` بعده — EF بيتجاهل الـ includes مع الـ projection)
- ❌ **ممنوع جوه الـ Specification:** Skip / Take / حسابات الصفحات — فيه unit test بيمسك أي حد يخالف ده بنيوياً

**مثال — Query متعدد الصفحات من أول لآخره:**

```csharp
// 1) Specification — Features/{Module}/Specifications/
public sealed class SubjectSearchSpecification : Specification<Subject>
{
    public SubjectSearchSpecification(string? search, Guid? yearId, Guid? semesterId)
    {
        if (yearId is not null)      Where(s => s.YearId == yearId);
        if (semesterId is not null)  Where(s => s.SemesterId == semesterId);
        if (!string.IsNullOrWhiteSpace(search))
            Where(s => s.Name.Contains(search) || s.Code.Contains(search));

        OrderBy(s => s.Name);   // ← لازم ترتيب لو الناتج هيتصفح (صفحات ثابتة)
    }
}
```

```csharp
// 2) الـ Handler — يطبّق الـ spec ثم الـ projection ثم الـ paging
var spec = new SubjectSearchSpecification(request.Search, request.YearId, request.SemesterId);

var page = await spec.ApplyTo(context.Subjects.AsNoTracking())
    .Select(s => new SubjectListItemResponse(s.Id, s.Code, s.Name))   // projection (بغنى عن Include)
    .ToPagedResultAsync(request, cancellationToken);

return Result.Success(page);
```

**القواعد:**
1. الـ spec اللي ناتجها هيتصفح **لازم** يحدد `OrderBy` — صفحات من غير ترتيب مش مضمونة الثبات.
2. `TotalCount` بيطلع تلقائياً = عدد النتائج **بعد الفلترة وقبل الـ paging** (مثال: 157 نتيجة مطابقة و page=2 و pageSize=10 → `TotalCount=157, TotalPages=16, Items 11-20`).
3. الـ validators بتاعة الـ queries المتصفحة بتفضل تسحب قواعد الـ paging بـ `Include(new PagedRequestValidator())` زي ما هي.
4. الـ specs بتاعة كل موديول تعيش في `Features/{Module}/Specifications/` — والأساس في Common ملك للجميع.

---

## 5) Entities جديدة

```csharp
// Guid key + Guid v7 يتبني أوتوماتيك + CreatedAt/UpdatedAt — كل اللي محتاجه
public sealed class Book : GuidKeyEntity
{
    public string Title { get; set; } = string.Empty;
}

// الـ Id قابل للتعيين — مفيد للـ Seed بقيم ثابتة:
var book = new Book { Id = Guid.Parse("00000000-0000-0000-0000-000000000001"), Title = "..." };
```

- محتاج key نوع تاني؟ `BaseEntity<TId>`.
- `BaseEntity` القديمة (= int) فضلت موجودة للتوافق — **متستخدمهاش في موديولات جديدة**.

---

## 6) مرجع سريع لكل نوع خطأ

| `ErrorType` | HTTP | امتى |
|---|---|---|
| `Error.Validation(details)` | 400 + `errors` | أخطاء حقول طلعت من الـ business logic |
| `Error.NotFound(...)` | 404 | المورد مش موجود |
| `Error.Forbidden(...)` | 403 | عنده حساب بس مش مسموح (بعد ما الـ Auth يجي) |
| `Error.Conflict(...)` | 409 | التعارض مع الحالة الحالية (duplicate / in-use) |
| `Error.Failure(...)` | 500 | فشل متوقع نادر (يفضل نادراً جداً) |

**الـ Exceptions مكانها من حقها في:** الـ unexpected failures (بيضبطها الـ middleware)، والـ `ValidationBehavior` (بيرمي `ValidationException` تلقائياً قبل ما الـ Handler يشوف الـ request). كل اللي غير كده → `Result`.

---

*أسئلة؟ كلّم [عضو 2]. الملف ده جزء من موديول الكتالوج — عدّله براحتك لو لقيت حاجة ناقصة.*
