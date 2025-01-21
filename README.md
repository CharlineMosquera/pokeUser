# BaconGames.PokeUser

## Descripción del Proyecto

BaconGames.PokeUser es una API para gestionar usuarios y Pokémon, utilizando ASP.NET Core 8 y MongoDB Atlas. La API permite el registro de usuarios, autenticación mediante JWT, agregar Pokémon al inventario de un usuario y consultar el inventario de Pokémon de un usuario autenticado.

## Arquitectura

El proyecto sigue los principios de Clean Architecture y la Regla de Dependencia, asegurando una separación clara de responsabilidades entre las distintas capas. La arquitectura facilita la escalabilidad y el mantenimiento a largo plazo.

### Capas del Proyecto

1. **API (BaconGames.PokeUser.Api)**: Esta capa contiene los controladores y la configuración de la API. Se encarga de recibir las solicitudes HTTP y devolver las respuestas HTTP. Es la capa que interactua con el cliente. Se enfoca en manejar las peticiones, invocar los servicios(desde la capa Application), y devolver las respuestas adecuadas.

2. **Infrastructure (BaconGames.PokeUser.Infrastructure)**: 
    + **BaconGames.PokeUser.External:** Contiene la implementación de los servicios externos, como la PokeAPI para obtener informacion del Pokemon.
    + **BaconGames.PokeUser.Persistence:** Contiene la implementación de la base de datos y el código relacionado con la persistencia de datos. Contiene los documentos para las entities del domain.

3. **Core (BaconGames.PokeUser.Core)**:
    + **BaconGames.PokeUser.Application:** Contiene la lógica de negocio, es donde se implementan los servicios gestiona el flujo de negocio (como el registro, login, validación de usuario, manejo de tokens, etc.). Se comunica con la capa Domain para obtener información relacionada con las entidades de dominio.
    + **BaconGames.PokeUser.Domain:** Esta capa contiene las entidades de dominio y las reglas de negocio. Es independiente de cualquier tecnología de infraestructura y no tiene dependencias hacia otras capas.

4. **Core (BaconGames.PokeUser.Common)**: Disponibe para albergar utilidades comunes, como clases auxiliares, extensiones, constantes y configuraciones compartidas entre las diferentes capas.

### Decisiones Técnicas

1. **Clean Architecture**: La arquitectura limpia asegura que las dependencias fluyan desde las capas externas hacia las capas internas, siguiendo la Regla de Dependencia. Esto facilita el mantenimiento y la escalabilidad del proyecto.

2. **MongoDB Atlas**: Se utiliza MongoDB Atlas como base de datos NoSQL para almacenar los datos de usuarios y Pokémon. MongoDB es una base de datos flexible y escalable.

3. **Autenticación con JWT**: Se utiliza JSON Web Tokens (JWT) para la autenticación de usuarios. Los tokens JWT se generan al iniciar sesión y se utilizan para autenticar las solicitudes a la API. La configuración de JWT se realiza en `Program.cs`.

4. **Hash de Contraseñas con BCrypt**: Las contraseñas de los usuarios se almacenan de forma segura utilizando el algoritmo de hash BCrypt. Esto asegura que las contraseñas no se almacenen en texto plano y proporciona una capa adicional de seguridad.

5. **Uso de Cookies para el Token**: El token JWT se almacena en una cookie segura para facilitar la autenticación en solicitudes subsecuentes. Esto mejora la seguridad al evitar que el token se exponga en las URL.

### Manejo de secretos

Se utiliza dotnet user-secrets para manejar la configuración sensible de forma segura.

### Ejecución

Clonar el repositorio

````
git clone https://github.com/CharlineMosquera/pokeUser.git
````
**IMPORTANTE**
***En el archivo appsettings.json debe configurar la url de conexion a Mongo y una clave secrete de minimo 256 bits para la firma de los tokens jwt, en el archivo appsettings.json.example encontrara un ejemplo***

Para ejecutar el proyecto, usa el siguiente comando:
````
dotnet run --project src/BaconGames.PokeUser.Api
````

### Swagger

Puede consultar la documentación de la API ejecutando la aplicación y navegando a:
````
http://localhost:<puerto>/swagger
````
### Pruebas con Postman (Herramienta similar)
Actualmente, la autorización de los endpoints no se configura automáticamente a través de la cookie tokenjwt. Por lo tanto, para probar la funcionalidad completa de la API, sigue estos pasos:

1. Realice un login para obtener el token JWT, el cual se almacena automáticamente en una cookie llamada tokenjwt.
2. Copie el valor del token desde la cookie tokenjwt.
3. En Postman, vaya a la pestaña de Autorización y seleccione el tipo Bearer Token.
4. Pegue el valor del token en el campo correspondiente.

De esta forma, podrá probar los endpoints protegidos con el token de autorización.

***Nota: La automatización de la autorización basada en cookies está en desarrollo y se implementará en futuras versiones.***



