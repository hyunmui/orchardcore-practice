# 아키텍처 문서

## 전체 구조

```
┌────────────────────────────────────────────────────┐
│                   Client Browser                   │
└──────────────────────┬─────────────────────────────┘
                       │ HTTP
┌──────────────────────▼─────────────────────────────┐
│          ASP.NET Core 8 Web Host                   │
│          OrchardCore.Practice.Web                  │
│                                                    │
│  ┌──────────────┐  ┌───────────────────────────┐  │
│  │ Orchard Core │  │ Exhibition Modules        │  │
│  │ CMS Engine   │  │ ┌─────────────────────┐  │  │
│  │              │  │ │ Exhibition.Core      │  │  │
│  │ - Tenants    │  │ │ (위젯, 콘텐츠 타입) │  │  │
│  │ - Content    │  │ ├─────────────────────┤  │  │
│  │ - Media      │  │ │ Exhibition.AI        │  │  │
│  │ - Flows      │  │ │ (Semantic Kernel)    │  │  │
│  │ - Liquid     │  │ ├─────────────────────┤  │  │
│  └──────────────┘  │ │ Exhibition.         │  │  │
│                    │ │ MultiTenant         │  │  │
│                    │ └─────────────────────┘  │  │
│                    └───────────────────────────┘  │
│                                                    │
│  ┌──────────────────────────────────────────────┐  │
│  │ ExhibitionTheme (Tailwind CSS + Liquid)      │  │
│  └──────────────────────────────────────────────┘  │
└──────────────────────┬─────────────────────────────┘
                       │
          ┌────────────┴────────────┐
          │                         │
┌─────────▼──────────┐   ┌─────────▼──────────┐
│   SQLite (개발)    │   │ SQL Server (운영)  │
└────────────────────┘   └────────────────────┘
```

## 멀티테넌시 아키텍처

```
Default Tenant (관리 전용)
├── 테넌트 관리 Admin
├── 공유 미디어 (계획)
└── 공통 콘텐츠 타입 정의

Tenant-A (도메인: exhibition-a.example.com)
├── ExhibitionPage 콘텐츠
├── ExhibitorProfile 콘텐츠
├── 독립적 미디어 라이브러리
└── ExhibitionTheme 적용

Tenant-B (도메인: exhibition-b.example.com)
├── ExhibitionPage 콘텐츠
├── ExhibitorProfile 콘텐츠
├── 독립적 미디어 라이브러리
└── ExhibitionTheme 적용
```

## AI 페이지 생성 흐름

```
Designer (Admin UI)
    │ 자연어 입력
    ▼
PageGeneratorController
    │ IPageGeneratorService.GeneratePageAsync()
    ▼
PageGeneratorService
    │ Semantic Kernel 초기화
    │ System Prompt (PageGenerator.txt) 로드
    │
    ▼
Claude API (claude-sonnet-4-6)
    │ JSON 응답
    ▼
PageGenerationResult 파싱
    │ Widget 목록
    ▼
Admin UI 미리보기
    │ 승인
    ▼
Orchard Core ContentManager
    │ ExhibitionPage + FlowPart 저장
    ▼
Published Page
```

## 위젯 렌더링 파이프라인

```
HTTP Request
    │
    ▼
OrchardCore Router
    │ ContentItem 조회
    ▼
ContentItemDisplayManager
    │ ContentPartDisplayDriver 탐색
    ▼
HeroBannerPartDisplayDriver (예시)
    │ ViewModel 생성
    ▼
Liquid Template Engine
    │ HeroBannerPart.liquid 렌더링
    ▼
ExhibitionTheme Layout.liquid
    │ 전체 레이아웃에 삽입
    ▼
HTML Response
```

## 보안 고려사항

- **API 키 관리**: User Secrets (개발) / 환경변수 (운영), 코드/설정 파일 하드코딩 금지
- **CSRF 보호**: Orchard Core 내장 Anti-forgery token 사용 (ContactFormWidget)
- **인증/인가**: Orchard Core 내장 사용자 관리 시스템 활용
- **컨테이너 보안**: Docker non-root 사용자(`appuser`)로 실행

## 배포 파이프라인

```
PR 생성
    │
    ▼
ci.yml 실행
├── dotnet restore
├── dotnet build
├── dotnet test
└── Docker 이미지 빌드 검증
    │ 통과
    ▼
main 브랜치 merge
    │
    ▼
deploy.yml 실행
├── Docker 이미지 빌드
├── GitHub Container Registry push
└── GitHub Deployment API 알림
```

## 콘텐츠 타입 관계

```
ExhibitionPage
└── FlowPart
    ├── HeroBannerWidget (0..*)
    ├── ExhibitorCardWidget (0..*)
    ├── ProductGalleryWidget (0..*)
    ├── ScheduleWidget (0..*)
    └── ContactFormWidget (0..*)

ExhibitorProfile
└── ExhibitorCardPart

ExhibitionEvent
└── SchedulePart

Announcement
└── HtmlBodyPart
```
