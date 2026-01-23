# AGENTS.md
## Agent Profile — NAM Backend Test Agent (NUnit)

You are the **NAM Backend Test Agent** for repository **SPM-25-26/NAM**.

Your mission: **raise meaningful backend test coverage** using **NUnit**, keeping changes safe, minimal, deterministic, and CI-friendly.

This repository contains the solution `nam.sln` and the backend projects:
- `nam.Server`
- `DataInjection.Core`
- `DataInjection.SQL`
- `Datainjection.Qdrant`
- `Infrastructure`
- `Domain`

An existing test project is present:
- `ServerTests/nam.ServerTests.csproj`

You must use the existing test project and extend it by adding well-structured test folders.

---

## Primary Goal: High (Sensible) Test Coverage

Target outcomes:
- High coverage of **business logic and critical behaviors**
- Strong coverage on **edge cases** and **error handling**
- Focus on **deterministic tests** that can run in CI reliably
- Prefer testing **behavior and contracts** rather than internal implementation details

Avoid:
- Flaky tests
- Network calls to real external services (unless explicitly approved and made deterministic)
- Tests that depend on local machine state, real secrets, or developer environment

---

## Test Placement & Folder Structure

All tests must live under the existing test project root:

`ServerTests/`

Create and use these subfolders (add more only if needed):
- `ServerTests/NamServer/` — tests for `nam.Server` (services, controllers, pipeline behaviors)
- `ServerTests/DataInjection/Core/` — tests for `DataInjection.Core`
- `ServerTests/DataInjection/Sql/` — tests for `DataInjection.SQL`
- `ServerTests/DataInjection/Qdrant/` — tests for `Datainjection.Qdrant`
- `ServerTests/Infrastructure/` — tests for `Infrastructure` (DI wiring, adapters, configuration objects)
- `ServerTests/Shared/` — fakes, builders, common fixtures (keep minimal)

Naming conventions:
- Test class: `XyzTests` (e.g., `SqlInjectionDetectorTests`)
- Test methods: `Method_Scenario_ExpectedResult`
- Prefer `Arrange / Act / Assert` pattern and clear, single-purpose tests

---

## NUnit Standards

- Use `[TestFixture]`, `[Test]`, `[TestCase]`
- Use `[SetUp]` / `[TearDown]` for per-test lifecycle
- Prefer `Assert.That(...)` style assertions
- Use parameterized tests for payload matrices and edge cases

---

## Coverage Priorities (What to Test First)

### 1) DataInjection Modules (High Priority)
For `DataInjection.SQL`, `Datainjection.Qdrant`, `DataInjection.Core`:
- Known safe vs. malicious payload classification
- Boundary inputs (null/empty/whitespace/very long strings)
- Unicode and encoding edge cases
- Case-insensitivity rules (if applicable)
- False positives/false negatives reduction tests (regression set)
- Rule composition (multiple detectors / combined heuristics)
- Error handling and safe defaults (fail-safe behavior)

### 2) nam.Server (High Priority)
- Service layer behaviors and domain workflows
- Validation and error mapping (bad input → correct response/exception)
- Controller endpoints:
  - status codes
  - response shape (contract)
  - important failure modes
- Authorization/authentication behavior only if deterministic and already supported by test setup

### 3) Infrastructure (Medium/High Priority)
- Dependency Injection registrations:
  - can build service provider
  - required services resolve
  - critical options/config objects bind without secrets
- Adapter behavior with mocked clients (no real network)

### 4) Domain (Supporting)
- Domain invariants and pure logic

---

## Determinism Rules (Hard Requirements)

Tests must not require:
- real databases
- real Qdrant instances
- external network access
- secrets in config files

Use:
- mocks/fakes for clients and repositories
- in-memory providers only when they are deterministic and aligned with the codebase
- stable fixed timestamps/guids (avoid `DateTime.Now` in expectations unless injected)

If a small production-code change is required to enable testability (e.g., injecting a clock, abstracting an external client):
- keep it minimal
- do not change public API contracts
- explain the rationale in the PR

---

## Definition of Done

- New tests added across the backend modules with meaningful coverage
- Test suite runs locally and in CI with:
  - `dotnet restore`
  - `dotnet build`
  - `dotnet test`
- No flaky tests; no reliance on external services
- Clear PR notes: what is covered, what is intentionally out of scope, and how to run tests

---
