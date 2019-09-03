![PaxMan](http://blog.indiegala.com/wp-content/uploads/2018/06/7-pac-man-fun-facts.jpg)

# **`PaxMan` a `Pac-Man` clone game**
### **Introducción:**
Este `GDD` fue creado en base a varias documentaciones y ejemplos, ya que encontrar un documento de diseño completo o que cubra las partes base del juego es compleja y la información difusa, por ende decidi la creación del mismo.

#### Objetivos del proyecto:
* >Termina el proyecto original `"Clon de Pac-Man"`.
* >Utilizar los mejores recursos de [Unity Engine](https://unity.com/).
* >Optimizar el código.
* >Refactorizar o cambiar la estructura del código si fuera necesario.
* >Documentar los cambios.

**Enlace documentación referencia:**
[Pac-Man wikipedia](https://es.wikipedia.org/wiki/Pac-Man)
---
---
## *"Analisis del código"*
Lo primero que realicé fue un analisis del código, para encontrar bugs e inconsistencias, los bugs más graves eran los lógicos, de sintaxis no había nada ya que el editor no daba errores en consola, pero el juego en si no funcionaba correctamente, habian lineas de código de mas, faltas de implementaciones y una programación orientada a objetos muy pobre, la cual desaprovechar todo el poder del motor, así que la decisión final es la refactorización y un poco de reestructuración de código implementando patrones de diseño donde fuera posible hacerlo, y no sobrecargar a una sola clase con demasiadas tareas... para no hacer demasiado larga la explicación se va a ir detallando a medida que la mecánica del juego se desarrolle.

---
## **El juego**
El protagonista es un círculo amarillo al que le falta un sector (Pac-Man), por lo que parece tener boca. Aparece en laberintos donde debe comer puntos pequeños (Pac-dots), puntos mayores y otros premios con forma de frutas. El objetivo del personaje es comer todos los puntos de la pantalla, para pasar de nivel. Sin embargo, cuatro fantasmas, recorren el laberinto para intentar capturar a Pac-Man. Estos fantasmas son, respectivamente, de colores rojo, rosa, cian y naranja.

Los fantasmas no son iguales, así mientras uno es muy rápido, y tiene la habilidad de encontrar a Pac-Man en el escenario, otro es muy lento y muchas veces evitará el encuentro con Pac-Man.

Hay un "pasillo" a los costados del laberinto que permiten a Pac-Man o sus enemigos transportarse al costado opuesto (sale por la derecha y reingresa por la izquierda, o viceversa). Cuatro puntos más grandes de lo normal situados cerca de las esquinas del laberinto nombrados «Power Pellets», proporcionan a Pac-Man, durante un tiempo limitado, la habilidad de comerse él a los monstruos (todos ellos se vuelven azules mientras Pac-Man tiene esa habilidad), tras lo cual todo vuelve a ser como al principio.

Después de haber sido comidos por Pac-Man, los fantasmas se regeneran en «casa» (una caja situada en el centro del laberinto). El tiempo en que los monstruos permanecen vulnerables varía según la pantalla, pero tiende a decrecer a medida que progresa el juego.

Además de comer los puntos, Pac-Man puede obtener puntuación adicional si se come alguno de los objetos que aparecen dos veces por pantalla justo debajo de la caja en el centro del laberinto de donde salen los monstruos. El objeto cambia cada pantalla o dos, y su valor en puntos aumenta.

`Extración del enlace anteriormente provisto`.

---

# **Game Design Document** *(Improvisado)*

## Game Mechanic
* [ ] `Main Screen`.
    * >Pantalla basica que avierte al jugador que tecla presionar para empezar el juego.

* [ ] `Mapa` (Originalmete es un archivo de texto, esto se va a mantener asi).
    * >Se van a usar varios archivos `.txt` para generar diferentes niveles (no procedural para dar un mejor control).
    * >Va a cambiar cada vez que se pase de nivel.
    * >Van a tener "Salidas" en los laterales que van a conectarse entre si para que Pac-Man y los fantasmas se transporten.

* [ ] `Comportamiento del main character`.
    * >Se movera (constantemente hasta que se tope con una pared) de forma horizontal o vertical dependiendo de las teclas que el usuario use:
        * >`"W"` o `"Direccional arriba"` para moverse hacia arriba.
        * >`"S"` o `"Direccional abajo"` para moverse hacia abajo.
        * >`"A"` o `"Direccional izquierda"` para moverse hacia izquierda.
        * >`"D"` o `"Direccional arriba"` para moverse hacia derecha.
    * >Comer "Puntos amarillos" para completar el nivel.
    * >Morir y reaparecer si es tocado por un fantasma hasta que no queden mas vidas.
    * >Si come un "Punto amarillo grande" este pasa de ser "presa" a "cazador".

* [ ] `Ojetos` (interactuan por colision)
    * >`Puntos amarillos`: otorgan 10 puntos.
    * >`Puntos amarillos grandes`: otorgan 10 puntos y la habilidad de comer fantasmas.
    * >`Transportadores`: en los laterales conectados entre si para que se transporten.

* [ ] `Los antagonistas`
    * >`Blinky` (fantasma rojo). Después de que Pac-Man coma cierta cantidad de puntos, su velocidad incrementa considerablemente (este número disminuye en niveles más altos).
    * >`Pinky` (fantasma rosa). Rodea los obstáculos al contrario de las manecillas del reloj. Esta suele colocarse en frente de pacman y cortarle el paso u orbita alrededor de Pac-Man para confundir y distraer al jugador para que uno de sus compañeros lo atrape.
    * >`Inky` (fantasma cian). No es tan rápido como Blinky pero actúa de manera errática así que es difícil predecir cómo va a reaccionar. En el juego original de Japón (Puck-Man) este fantasma solía evitar a Pac-Man y era considerado temeroso.
    * >`Clyde` (fantasma naranja). Él no persigue a Pac-Man, sino que deambula por el laberinto sin una ruta específica.

* [ ] `Victoria`
    * >Si se consigue pasar todos los niveles se muestra una pantalla de victoria.
    * >Vuelve al `Main Screen`
* [ ] `Derrota`
    * >Si se pierden todas las vidas se muestra un `Game Over`.
    * >Vuelve al `Main Screen`.