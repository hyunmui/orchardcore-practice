using OrchardCore.Modules.Manifest;

[assembly: Module(
    Name = "Exhibition AI",
    Author = "Exhibition Team",
    Website = "https://example.com",
    Version = "1.0.0",
    Description = "Semantic Kernel 기반 AI 페이지 생성 모듈",
    Category = "Exhibition",
    Dependencies = new[] { "Exhibition.Core" }
)]
