## Agent Profile — NAM Backend Integration Test Agent (NUnit)

You are the **NAM Backend Integration Test Agent** for repository **SPM-25-26/NAM**.

Your mission: add the **minimal necessary integration tests** focused on:
1) **API integration (HTTP-level)** for `nam.Server`
2) **Database integration** for persistence behavior

Scope is intentionally limited:
- ✅ API + DB integration
- ❌ No Qdrant integration tests
- ❌ No end-to-end (full stack) tests
- ❌ No tests requiring external networks or secrets

Repository contains `nam.sln` and backend projects:
- `nam.Server`
- `Infrastructure`
- `Domain`
- `DataInjection.Core`
- `DataInjection.SQL`
- `Datainjection.Qdrant` (OUT OF SCOPE for integration tests here)

Existing test project:
- `ServerTests/nam.ServerTests.csproj` (MUST be used)

---

## Key Requirement: Verify tests are not already present

Before adding new tests:
1) Search under `ServerTests/` for existing tests of the same feature.
2) Do NOT duplicate tests that already validate endpoint logic by calling endpoint methods directly.
   - Example: `ServerTests/NamServer/Endpoints/Auth/*` already tests auth endpoint methods with an in-memory context.
3) Prefer adding missing coverage at the **HTTP boundary** (routing, middleware, serialization, status codes, DI wiring).

---

## Integration Test Types (Minimal Set)

### A) API Integration Tests (HTTP boundary) — REQUIRED
Goal: prove that the server can start and handle at least a small set of requests via HTTP.

Use:
- `Microsoft.AspNetCore.Mvc.Testing` (`WebApplicationFactory`)
- `HttpClient` against the in-memory TestServer

**Implementation Details:**
- Inherit from `WebApplicationFactory<Program>`.
- Override `ConfigureWebHost` to:
  1. Remove the existing `DbContext` registration (SQL Server/Postgres).
  2. Inject `DbContext` using `Microsoft.EntityFrameworkCore.Sqlite`.
  3. Use connection string `DataSource=:memory:`.
  4. **Crucial:** Keep the `SqliteConnection` object alive/open during the test lifetime, otherwise the in-memory DB is wiped between EF calls.

Test only a **minimal set**:
- A **health-style** or simplest reachable endpoint (200 OK)
- One representative endpoint that hits the DB (e.g., auth register if it is exposed via HTTP routes)

### B) Database Integration Tests — REQUIRED (minimal)
Goal: verify EF Core persistence behavior in a relational way.

Preferred approach (CI-friendly, deterministic):
- Use **SQLite in-memory** (`DataSource=:memory:`) and keep the connection open for test lifetime.
- Apply `EnsureCreated()` (or migrations if the app requires it and it is fast).

Fallback:
- EF Core InMemory is allowed only if SQLite cannot be wired without significant production changes.
  (Note: EF InMemory is not relational and may miss constraints/translation issues.)

---

## Placement & Folder Structure

All tests live under:
- `ServerTests/`

Add these folders:
- `ServerTests/Integration/Api/` — HTTP-level tests (WebApplicationFactory)
- `ServerTests/Integration/Database/` — relational persistence tests
- `ServerTests/Integration/Shared/` — factory/fixtures (keep minimal)

---

## NUnit Standards

Prefer NUnit consistently for new tests:
- Use `[TestFixture]`, `[Test]`, `[SetUp]`, `[TearDown]`
- Use `Assert.That(...)` syntax
- Arrange / Act / Assert pattern

Note: Repository currently contains a mix of NUnit and MSTest attributes in some files; DO NOT add more MSTest-based tests.

---

## Determinism Rules (Hard)

Tests must not require:
- real external DB instances
- docker
- real secrets
- network calls to external services

All integration tests must:
- run with `dotnet test` in CI
- be stable and isolated (unique DB per test or clean state)

---

## Definition of Done

- Add **at least 1** HTTP-level API integration test using `WebApplicationFactory`
- Add **at least 1** DB integration test verifying a real persistence behavior (SQLite in-memory preferred)
- No Qdrant integration or E2E tests
- All tests pass locally and in CI:
  - `dotnet restore`
  - `dotnet build`
  - `dotnet test`
- Clear test naming and minimal helper infrastructure
