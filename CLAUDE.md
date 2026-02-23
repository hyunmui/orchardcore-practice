# Orchard Core Practice — Claude Code 지시문

## 프로젝트 개요
전시회용 웹사이트 CMS 프로토타입.
WordPress(Avada 기반) 대체를 목표로 하며 아래 핵심 요구사항을 충족해야 한다.

- 단일 코드베이스로 멀티 도메인 운영 (원소스 멀티유즈)
- 웹 디자이너가 코드 없이 페이지를 구성할 수 있는 비주얼 빌더
- AI 에이전트를 통한 페이지 자동 생성
- 현대적 배포 파이프라인 (Docker + GitHub Actions)

---

## 기술 스택

| 레이어 | 기술 |
|---|---|
| 프레임워크 | ASP.NET Core 8 + Orchard Core 2.x (최신 stable) |
| AI 오케스트레이션 | Microsoft Semantic Kernel |
| LLM | Anthropic Claude API (claude-sonnet-4-6) |
| DB | SQLite (개발) / SQL Server (운영) |
| 컨테이너 | Docker + docker-compose |
| CI/CD | GitHub Actions |
| 패키지 관리 | NuGet (dotnet CLI 기반) |

---

## 프로젝트 구조

아래 구조를 반드시 따를 것.

```
orchardcore-practice/
├── src/
│   ├── OrchardCore.Practice.Web/          # 메인 웹 호스트
│   ├── OrchardCore.Practice.Modules/      # 커스텀 모듈
│   │   ├── Exhibition.Core/               # 전시회 공통 컨텐츠 타입
│   │   ├── Exhibition.AI/                 # Semantic Kernel AI 에이전트
│   │   └── Exhibition.MultiTenant/        # 멀티테넌시 확장
│   └── OrchardCore.Practice.Themes/       # 커스텀 테마
│       └── ExhibitionTheme/
├── tenants/                               # 테넌트별 설정 파일
│   ├── tenant-a/
│   └── tenant-b/
├── docker/
│   ├── Dockerfile
│   └── docker-compose.yml
├── .github/
│   └── workflows/
│       ├── ci.yml
│       └── deploy.yml
├── docs/
│   └── architecture.md
└── CLAUDE.md
```

---

## 핵심 구현 요구사항

### 1. 멀티테넌시 (Multi-Tenancy)

- Orchard Core의 `OrchardCore.Tenants` 모듈을 활성화할 것
- 각 테넌트는 독립적인 도메인, 테마, 콘텐츠를 가져야 함
- Default 테넌트는 테넌트 관리 전용 Admin으로만 사용
- 테넌트 생성 시 Exhibition 기본 레시피(Recipe)가 자동 적용되어야 함
- 테넌트 간 공유 리소스(미디어, 공통 콘텐츠 타입)는 Default 테넌트에서 관리

```csharp
// 테넌트 설정 예시 — 이 패턴을 따를 것
services.AddOrchardCms()
    .AddTenantFeatures(
        "OrchardCore.Tenants",
        "OrchardCore.Admin",
        "OrchardCore.ContentTypes",
        "OrchardCore.Flows",          // 페이지 빌더
        "OrchardCore.Media",
        "OrchardCore.Apis.GraphQL"
    );
```

### 2. 페이지 빌더 (디자이너 자립도)

- `OrchardCore.Flows` 모듈을 기본 활성화할 것
- 아래 커스텀 위젯을 `Exhibition.Core` 모듈에 반드시 구현할 것
  - `HeroBannerWidget` : 전시 메인 배너 (이미지 + 제목 + CTA 버튼)
  - `ExhibitorCardWidget` : 참가업체 카드 (로고 + 업체명 + 설명 + 링크)
  - `ProductGalleryWidget` : 제품 갤러리 (N열 그리드, 열 수 설정 가능)
  - `ScheduleWidget` : 전시 일정표
  - `ContactFormWidget` : 문의 폼

- 각 위젯은 `IDisplayDriver`를 구현하고 Liquid 템플릿으로 렌더링할 것
- 디자이너가 위젯 내 텍스트/이미지를 Admin UI에서 직접 수정 가능해야 함

### 3. AI 에이전트 (Exhibition.AI 모듈)

Semantic Kernel을 사용하여 자연어로 페이지를 생성하는 에이전트를 구현할 것.

**구현할 기능:**
- Admin UI에 "AI 페이지 생성" 메뉴 추가
- 디자이너가 자연어로 페이지 구조를 입력
- AI가 적절한 위젯 조합과 콘텐츠를 생성
- 미리보기 후 승인하면 실제 페이지로 저장

**필수 구현 파일:**

```csharp
// IPageGeneratorService 인터페이스 반드시 구현
public interface IPageGeneratorService
{
    Task<PageGenerationResult> GeneratePageAsync(
        string naturalLanguagePrompt,
        string tenantId,
        CancellationToken cancellationToken = default);
}

// PageGenerationResult 구조
public class PageGenerationResult
{
    public string Title { get; set; }
    public string Permalink { get; set; }
    public List<WidgetDefinition> Widgets { get; set; }
    public bool Success { get; set; }
    public string ErrorMessage { get; set; }
}
```

**Semantic Kernel 설정:**
- `appsettings.json`에 AI 설정 섹션을 분리할 것
- API Key는 환경변수 또는 User Secrets로만 관리 (코드에 하드코딩 금지)
- Claude API 호출 시 System Prompt는 `Exhibition.AI/Prompts/` 폴더의 `.txt` 파일로 관리

**System Prompt 요구사항 (PageGenerator.txt):**
```
- 반환 형식은 반드시 JSON
- 사용 가능한 위젯 목록을 컨텍스트로 주입
- 위젯 순서와 각 위젯의 초기 콘텐츠 포함
- 한국어 입력을 기본으로 처리
```

### 4. 콘텐츠 타입 (Exhibition.Core)

아래 콘텐츠 타입을 Recipe JSON으로 정의할 것.

| 콘텐츠 타입 | 필드 | 설명 |
|---|---|---|
| `ExhibitionPage` | Title, FlowPart, AutoroutePart | Flow 빌더로 구성되는 일반 페이지 |
| `ExhibitorProfile` | Title, LogoMediaField, Description, WebsiteUrl, Category | 참가업체 프로필 |
| `ExhibitionEvent` | Title, StartDate, EndDate, Location, Description | 전시 일정 |
| `Announcement` | Title, Summary, Body, PublishedDate | 공지사항 |

### 5. 테마 (ExhibitionTheme)

- Liquid 템플릿 기반으로 작성
- 반응형 (모바일 퍼스트)
- CSS는 Tailwind CSS CDN 사용 (빌드 환경 최소화)
- 레이아웃 파일: `Layout.liquid`, `Document.liquid`
- 위젯 템플릿은 `Views/` 하위에 위젯명으로 파일 분리

---

## 코딩 컨벤션

- 언어: **C# 12** 문법 사용 (primary constructor, collection expression 등 최신 기능 활용)
- 네이밍: Microsoft C# 코딩 컨벤션 준수
- 비동기: 모든 I/O 작업은 `async/await` 사용, `Task.Result` / `.Wait()` 금지
- 의존성 주입: 생성자 주입 원칙, `ServiceLocator` 패턴 금지
- 로깅: `ILogger<T>` 사용, `Console.WriteLine` 금지
- 예외 처리: 전역 예외 미들웨어 구성, 각 레이어에서 의미 있는 예외 타입 사용
- 테스트: 각 Service 클래스에 대해 xUnit 단위 테스트 작성

---

## Docker 구성

`docker-compose.yml`은 아래 서비스를 포함할 것.

```yaml
services:
  web:        # Orchard Core 앱
  db:         # SQL Server (운영 환경 시뮬레이션)
  adminer:    # DB 관리 UI (개발 전용)
```

- 환경변수는 `.env.example` 파일로 템플릿 제공
- 볼륨 마운트: App_Data, 미디어 파일 퍼시스턴스 보장
- health check 설정 포함

---

## GitHub Actions 워크플로우

### ci.yml (PR 시 자동 실행)
1. `dotnet restore`
2. `dotnet build --no-restore`
3. `dotnet test --no-build`
4. Docker 이미지 빌드 검증

### deploy.yml (main 브랜치 push 시)
1. CI 통과 확인
2. Docker 이미지 빌드 + GitHub Container Registry push
3. 배포 알림 (GitHub Deployment API)

---

## 구현 우선순위

Claude Code는 아래 순서로 구현할 것.
건너뛰거나 순서를 바꾸지 말 것.

1. [ ] 솔루션 및 프로젝트 구조 생성
2. [ ] 기본 Orchard Core 웹 호스트 설정 (멀티테넌시 포함)
3. [ ] Exhibition.Core 모듈 — 콘텐츠 타입 및 위젯 정의
4. [ ] ExhibitionTheme — 기본 레이아웃 및 위젯 템플릿
5. [ ] 기본 Recipe 파일 (테넌트 초기화용)
6. [ ] Exhibition.AI 모듈 — Semantic Kernel + Claude API 연동
7. [ ] AI 페이지 생성 Admin UI
8. [ ] Docker 구성
9. [ ] GitHub Actions 워크플로우
10. [ ] 단위 테스트
11. [ ] README.md 및 docs/architecture.md 작성

---

## 작업 시 주의사항

- Orchard Core NuGet 패키지는 항상 동일한 버전으로 맞출 것 (버전 충돌 방지)
- Recipe JSON 파일은 반드시 스키마 검증 후 작성
- 민감 정보(API Key, Connection String)는 절대 코드나 설정 파일에 하드코딩 금지
- 각 모듈은 독립적으로 활성화/비활성화 가능한 구조로 설계
- 새 파일 생성 시 프로젝트 `.csproj`에 등록 여부 확인
- 막히는 부분이 있으면 구현을 스킵하지 말고 TODO 주석과 함께 인터페이스/스텁을 먼저 작성할 것
