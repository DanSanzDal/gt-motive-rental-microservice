Microservicio de Gestión de Flota para Empresa de Renting
Este proyecto implementa un microservicio para la gestión de una flota de vehículos de renting, siguiendo los principios de Arquitectura Hexagonal y Domain-Driven Design.

Requisitos previos
Para ejecutar este proyecto necesitas tener instalado:

.- Docker
.- Docker Compose (incluido con Docker Desktop en Windows y Mac)
No se requiere ninguna otra dependencia ni instalar .NET localmente.

.- Instrucciones para ejecutar el proyecto
Descomprimir el archivo recibido en una carpeta de tu elección.

- Abrir un terminal o línea de comandos y navegar hasta la carpeta donde descomprimiste el proyecto:
  cd ruta/a/la/carpeta/del/proyecto

- Construir e iniciar los contenedores con Docker Compose:
  docker-compose up -d

- Este comando iniciará:

	.- El microservicio de API (gt-motive-estimate-api)
	.-Una base de datos MongoDB (gt-motive-mongodb)

- Verificar que los contenedores están en ejecución:
  docker ps

- Deberías ver dos contenedores activos.

Acceder a la documentación de la API (Swagger) abriendo la siguiente URL en tu navegador:
http://localhost:8080/index.html

Desde ahí puedes probar todas las funcionalidades de la API directamente.

.- Funcionalidades disponibles a través de la API:
	- Crear vehículos para la flota
	- Listar los vehículos disponibles	
	- Registrar clientes
	- Alquilar vehículos a clientes
	- Procesar devoluciones de vehículos


Visualizar logs, si necesitas ver los logs del microservicio:
	docker logs gt-motive-estimate-api

Detener el proyecto, cuando hayas terminado de revisar el proyecto:
	docker-compose down
	
Si deseas eliminar también los volúmenes de datos:
	docker-compose down -v
	
Notas adicionales
	- La base de datos MongoDB se inicializa automáticamente al arrancar
	- No se requiere configuración adicional para que todo funcione
	- Si encuentras algún problema al iniciar los contenedores, asegúrate de que los puertos 8080 y 27017 estén disponibles en tu sistema