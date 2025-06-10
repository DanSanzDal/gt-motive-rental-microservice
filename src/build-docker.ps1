# Construir la imagen Docker
Write-Host "Building Docker image for GtMotive Estimate Microservice..." -ForegroundColor Green

docker build -t gt-motive-estimate-microservice .

if ($LASTEXITCODE -eq 0) {
    Write-Host "Docker image built successfully!" -ForegroundColor Green
} else {
    Write-Host "Error building Docker image." -ForegroundColor Red
    exit 1
}