
<h1 align="center">
  <br>
  <a href=""><img src="img/logo_consejo.png" alt="Consejo Informática UMA" width="200"></a>
  <br>
  Aya
  <br>
</h1>

<h4 align="center">Discord bot developed for Board voting of the <a href="https://www.uma.es/etsi-informatica/info/126304/consejo-de-estudiantes/" target="_blank">Consejo de Informática of UMA</a> using <a href="https://github.com/discord-net/Discord.Net">Discord.Net</a>.</h4>

<p align="center">
  <img alt="GitHub last commit" src="https://img.shields.io/github/last-commit/GDUMA/Aya">
  <img alt="GitHub top language" src="https://img.shields.io/github/languages/top/GDUMA/Aya">
  <img alt="GitHub" src="https://img.shields.io/github/license/GDUMA/Aya">
  
</p>

<p align="center">
  <a href="#características">Features</a> •
  <a href="#instalación">Installation</a> •
  <a href="#configuración">Configuration</a> •
  <a href="#estados">States</a> •
  <a href="#roles">Roles</a> •
  <a href="#licencia">License</a>
</p>


## Características

* Permite votar a hasta 15 candidatos, asignándole a cada uno una puntuación única del 1 al 15.
* Rol de administrador
  - Encargado de manejar la votación
* Rol de votante
  - Podrán presentarse a candidato y votar
* Multiplataforma
  - Windows, macOS y Linux.

## Instalación

Para clonar y ejecutar el bot necesitarás [Git](https://git-scm.com) y [.NET Core 3.1](https://docs.microsoft.com/es-es/dotnet/core/install/windows?tabs=netcore31) instalado.

Puedes hacerlo ejecutando los siguientes comandos en un terminal:
```sh
# Clona este repositorio
git clone https://github.com/GDUMA/Aya

# Ve a la carpeta del proyecto
cd Aya

# Instala las dependencias
dotnet restore

# Ejecuta el bot
dotnet run
```

> También es posible ejecutar el bot desde tu IDE si este soporta el desarrollo en .NET

Para generar un ejecutable recomendamos seguir [la documentación de .NET Core](https://docs.microsoft.com/es-es/dotnet/core/deploying/deploy-with-cli) donde vienen explicadas las distintas formas de hacerlo.

## Configuración
En la primera ejecución se creará el archivo `config.json`, donde habrá que modificar el token del bot. 

Este es el contenido por defecto:

```json
{
  "Token": "your-token"
}
```
Para obtener tu token deberás primero tener una aplicación de Discord, la cual se puede crear en el [portal para desarrolladores de Discord](https://discord.com/developers/). El token se encuentra en el apartado _Bot_ dentro de la configuración de la aplicación.

## Estados
La votación dispone de varios estados definidos que permiten o no la ejecución de ciertas acciones por parte de los usuarios. La mayoría de los estados pueden ser controlados por el moderador:

- **Waiting**: La votación se acaba de crear, únicamente muestra el título y una descripción. A la espera del moderador para comenzar.
- **Registering**: Los miembros con el rol de _votante_ pueden presentarse a candidato reaccionando con 📝. Pueden cancelar si borran la reacción. Espera al moderador para avanzar de estado.
- **SendingMessages**: El bot comienza a enviar mensajes privados a todos los _votantes_ con un intervalo de 0.8 segundos. Al mismo tiempo el bot comenzará a leer los mensajes privados por si alguien comienza a votar. Avanzará al siguiente estado automáticamente una vez enviados los mensajes.
- **Voting**: En este estado se muestra la cantidad de votantes que han votado y es cuando se espera a que se termine de votar. El modarador es quien avanza al próximo estado.
- **ProcessingResults**: Literalmente no hace nada. El moderador tiene que avanzar al siguiente estado.
- **Finished**: La votación ha terminado y mostrará los resultados ordenados por puntuación.

## Roles
El interactuará únicamente con los usuarios que tengan los roles de moderador y votante explicados más adelante.
El ID de los roles está definido en  [`Constants.cs`](src/Constants.cs).
Para obtener el ID de un rol en Discord hay que mencionar el rol añadiendo una barra invertida al principio, `\@rol`. Esto escribirá un mensaje que mostrará el ID del rol siguiendo el formato `<@&ROL_ID>`, de donde se puede copiar el ID del rol.
### Moderador
Es el encargado de manejar la votación. Puede iniciar votaciones con el comando `newpoll <título>` y avanzar los estados descritos anteriormente reaccionando con ➡️, tal y como se indica en el archivo `Constants.cs`

### Votante
Los usuarios con este rol podrán votar y presentarse a candidato. Para presentarse a candidato deberá reaccionar con 📝 a la votació durante el estado _Registering_ . Podrar retractarse de su cadidatura eliminando la reacción mientras se siga en el mismo estado.
Cuando se pasa al siguiente estado, _SendingMessages_, recibirá las instrucciones y podrá comenzar a votar.
Tal y como se indica en dicho mensaje, el voto solo se guarda cuando se reacciona con ✅.
![voter message](img/votacion.png)

## Licencia
[MIT](LICENSE)
