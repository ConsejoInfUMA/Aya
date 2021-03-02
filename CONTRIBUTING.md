
<h1 align="center">CÓMO CONTRIBUIR EN EL PROYECTO</h1>

<p>
  Este documento describe cómo los contribuidores pueden hacer sus aportes con solicitudes de extracción a un repositorio ascendente.
</p>

<br>

- <a href="#fork">Fork del repositorio</a>
- <a href="#clonar">Clonar el repositorio</a>
- <a href="actualizar-rama">Actualizar la rama master</a>
- <a href="crear-rama">Crear una rama</a>
- <a href="modificaciones">Modificaciones</a>
- <a href="push-pull">Subir cambios</a>

<br>

<h2 name="fork">1. Fork del repositorio</h2>
<p>El primer paso es hacer "Fork" del repositorio.</p>

<h2 name="clonar">2. Clonar el repositorio</h2>
<p>
  Después de tener el repositorio en nuestra cuenta, seleccionar la dirección del repositorio "SSH o HTTP" y copiarla en el portapapeles, para luego clonar.
</p>
```
hhkjhjk
$ git clone https://github.com/[Usuario]/[Repositorio].git
hkjhk
```
<p>
  Dentro de la carpeta que genera, comprobar la URL del repositorio.
</p>
`$ git remote -v`
<p>
  Antes de realizar modificaciones, agregar la URL del repositorio original del proyecto (del cual se realizó el fork o bifurcación)
</p>
```
$ git remote add upstream https://github.com/User/[Repositorio-Original]
```
<p>
  Comprobar que las conexiones fueron aplicadas correctamente.
</p>
```
$ git remote -v
```
<h2 name="actualizar-rama">3. Actualizar la rama Master</h2>
<p>
  Antes de empezar a trabajar, obtener los últimos cambios del repositorio original.
</p>
```
$ git pull -r upstream master
```

<h2 name="crear-rama">4. Crear una Rama</h2>
<p>Para crear una rama usar la opción "checkout" de git.</p>
```
$ git checkout -b [Nombre-de-Rama]
```

<h2 name="modificaciones">5. Modificaciones</h2>
<p>
  Realizar todos los cambios que se desea hacer al proyecto.
  Agregar los archivos y hacer un commit.
</p>
```
$ git add .
$ git commit -m "[Mensaje-del-Commit]"
```
<h2 name="push-pull">6. Subir cambios</h2>
<p>
  Después de realizar el commit hacer el push hacia nuestro repositorio indicando la rama que hemos creado.
</p>
```
$ git push origin [Nombre-de-Rama]
```
<p>
Desde GitHub, hacer un Pull Request.
Hacer click en "Compare & Pull Request"
Escribir cambios del Pull Request.
Si todo está bien, enviar con el botón "Send Pull Request".

El encargado del repositorio debe aceptar sus cambios y unificar la rama que creó con la rama principal del proyecto.
</p>
