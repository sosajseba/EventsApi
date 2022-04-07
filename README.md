# API Eventos

_Este proyecto consta de una API que permite gestionar eventos en una ubicación y fecha especifícos_ 
Este proyecto consta de dos contenedores, uno para la API y otro para la base de datos. Gracias a esto no es necesario el uso de ningún script para la creación 
de la base de datos. Esta es creada y configurada automáticamente al iniciar los contenedores._

<!-- TABLE OF CONTENTS -->
<details>
  <summary>Contenido</summary>
  <ol>
    <li>
      <a href="#comenzando-">Comenzando</a>
      <ul>
        <li><a href="#pre-requisitos-">Pre-requisitos</a></li>
      </ul>
        <ul>
        <li><a href="#instalación-y-ejecución-">Instalación y ejecución</a></li>
      </ul>
    </li>
    <li><a href="#construido-con-%EF%B8%8F">Construido con</a></li>
    <li><a href="#contacto-">Contacto</a></li>
  </ol>
</details>

## Comenzando 🚀

### Pre-requisitos 📋

_Para ejecutar este proyecto es necesario contar con [Docker](https://docs.docker.com/desktop/windows/install/)_

### Instalación y ejecución 🔧

1. Clonar el repositorio
   ```sh
   git clone https://github.com/sosajseba/EventsApi.git
   ```
2. Configuracion   

   _Dentro del archivo `appsetings.json` editar el nodo "Authority" insertando la url de su proveedor de autenticación_
   
4. Build
   ```sh
   docker-compose build
   ```   
4. Ejecutar
   ```sh
   docker-compose up
   ```
5. En su navegador acceder a `http://localhost:8080/swagger/index.html` 

## Construido con 🛠️

* [.NET 6](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)
* [MongoDB Driver .NET](https://www.mongodb.com/docs/drivers/csharp/)
* [MongoDB](https://www.mongodb.com/)
* [Swagger](https://swagger.io/)
* [Docker](https://www.docker.com/)

## Contacto

* **Juan Sebastian Sosa** - [Linkedin](https://www.linkedin.com/in/juansebastiansosa/)

<p align="right">(<a href="#top">Volver arriba</a>)</p>
