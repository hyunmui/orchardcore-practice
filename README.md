# Orchard Core Practice — 전시회 CMS 프로토타입

WordPress(Avada 기반) 대체를 목표로 한 전시회용 웹사이트 CMS 프로토타입.
ASP.NET Core 8 + Orchard Core 2.x 기반으로 단일 코드베이스에서 멀티 도메인을 운영하며, AI 에이전트를 통한 페이지 자동 생성을 지원합니다.

## 기술 스택

| 레이어 | 기술 |
|---|---|
| 프레임워크 | ASP.NET Core 8 + Orchard Core 2.1.7 |
| AI 오케스트레이션 | Microsoft Semantic Kernel 1.30 |
| LLM | Anthropic Claude API (claude-sonnet-4-6) |
| DB | SQLite (개발) / SQL Server (운영) |
| 컨테이너 | Docker + docker-compose |
| CI/CD | GitHub Actions |
| 스타일 | Tailwind CSS (CDN) |

## 프로젝트 구조

```
orchardcore-practice/
├── src/
│   ├── OrchardCore.Practice.Web/          # 메인 웹 호스트
│   ├── OrchardCore.Practice.Modules/
│   │   ├── Exhibition.Core/               # 위젯, 콘텐츠 타입, Recipe
│   │   ├── Exhibition.AI/                 # Semantic Kernel AI 에이전트
│   │   └── Exhibition.MultiTenant/        # 멀티테넌시 확장
│   └── OrchardCore.Practice.Themes/
│       └── ExhibitionTheme/               # Tailwind CSS 기반 테마
├── tests/
│   └── Exhibition.Tests/                  # xUnit 단위 테스트
├── docker/
│   ├── Dockerfile
│   └── docker-compose.yml
├── .github/workflows/
│   ├── ci.yml                             # PR 자동 빌드/테스트
│   └── deploy.yml                         # main 브랜치 배포
└── docs/
    └── architecture.md
```

## 빠른 시작

### 로컬 개발 (SQLite)

```bash
# 의존성 복원
dotnet restore

# 개발 서버 실행
dotnet run --project src/OrchardCore.Practice.Web

# 브라우저에서 http://localhost:5000 접속 후 설치 마법사 진행
```

### Docker 실행

```bash
# 환경변수 파일 복사 및 수정
cp .env.example .env
# .env 파일에서 CLAUDE_API_KEY 등 실제 값으로 수정

# 서비스 시작 (web + db)
cd docker
docker-compose up -d

# 개발 모드 (adminer 포함)
docker-compose --profile dev up -d
```

## AI 페이지 생성

1. Orchard Core Admin 접속 (`/Admin`)
2. 좌측 메뉴에서 **AI 페이지 생성** 클릭
3. 원하는 페이지를 한국어로 설명 입력
4. **페이지 생성** 버튼 클릭

**예시 프롬프트:**
```
2024 스마트팩토리 전시회 메인 페이지를 만들어줘.
히어로 배너에는 '미래를 만드는 공장'이라는 제목을 넣고,
주요 참가업체 카드 섹션과 전시 일정표, 그리고 문의 폼을 추가해줘.
```

## AI API 키 설정

API 키는 **절대 코드에 하드코딩하지 마세요**.

**방법 1: User Secrets (개발)**
```bash
cd src/OrchardCore.Practice.Web
dotnet user-secrets set "AI:Claude:ApiKey" "sk-ant-..."
```

**방법 2: 환경변수**
```bash
export AI__Claude__ApiKey="sk-ant-..."
```

**방법 3: Docker .env 파일**
```
CLAUDE_API_KEY=sk-ant-...
```

## 위젯 목록

| 위젯 | 설명 |
|---|---|
| `HeroBannerWidget` | 전시 메인 배너 (이미지 + 제목 + CTA 버튼) |
| `ExhibitorCardWidget` | 참가업체 카드 (로고 + 업체명 + 설명 + 링크) |
| `ProductGalleryWidget` | 제품 갤러리 (N열 그리드, 열 수 설정 가능) |
| `ScheduleWidget` | 전시 일정표 |
| `ContactFormWidget` | 문의 폼 |

## 테스트 실행

```bash
dotnet test
```

## 라이선스

MIT
