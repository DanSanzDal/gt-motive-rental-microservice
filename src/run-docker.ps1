# Ejecutar los contenedores Docker
Write-Host "Starting Docker containers for GtMotive Estimate Microservice..." -ForegroundColor Green

docker-compose up -d

if ($LASTEXITCODE -eq 0) {
    Write-Host "Docker containers are now running!" -ForegroundColor Green
    Write-Host "API accessible at: http://localhost:8080" -ForegroundColor Cyan
    Write-Host "MongoDB accessible at: mongodb://localhost:27017" -ForegroundColor Cyan
    Write-Host "Mongo Express UI accessible at: http://localhost:8081" -ForegroundColor Cyan
    Write-Host "To stop, run: docker-compose down" -ForegroundColor Yellow
} else {
    Write-Host "Error starting Docker containers." -ForegroundColor Red
    exit 1
}