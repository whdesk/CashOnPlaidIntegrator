<img src="./upitn25u.png" style="width:4.09861in" /><img src="./f2pp30we.png"
style="width:4.09861in;height:0.10833in" /><img src="./tudmdgyq.png" style="width:4.09861in" /><img src="./1p0edc04.png"
style="width:4.11458in;height:1.54583in" />

> **Integración** **inSite**
>
> Versión: 3.0
>
> 18/04/2022
>
> RS.ADQUI.CNOPRESENCECOMM.LIST.0000
>
> Redsys · C/ Francisco Sancha, 12 · 28034 · Madrid · ESPAÑA
>
> Versión: 3.0 \<USO EXTERNO RESTRINGIDO\> 18/04/2022

<img src="./asvynvu4.png" style="width:4.09861in" />

> <img src="./fdqtnjb4.png"
> style="width:1.05208in;height:0.40625in" />***Integración***
> ***inSite***
>
> **Autorizaciones** **y** **control** **de** **versión**
>
> <img src="./q1kcay2h.png"
> style="width:4.61458in;height:4.61458in" /><img src="./2rz3tqx5.png" style="width:4.09861in" />Redsys
> · C/ Francisco Sancha, 12 · 28034 · Madrid · ESPAÑA
>
> Versión: 3.0 \<USO EXTERNO RESTRINGIDO\> 18/04/2022
>
> 1

<img src="./i1jn5piu.png" style="width:4.09861in" />

> <img src="./ialimoga.png"
> style="width:1.05208in;height:0.40625in" />***Integración***
> ***inSite***
>
> **ÍNDICE** **DE** **CONTENIDO**
>
> **1.** **Objetivo** **de** **esta** **guía**
> **...........................................................................4**
>
> **2.** **Conceptos** **y** **ventajas** **de** **la** **conexión**
> **inSite..........................................5**
>
> **3.** **Descripción** **general** **del**
> **flujo................................................................6**
>
> **4.** **Página** **de** **Pago** **–** **Obtención** **de** **ID**
> **de** **operación..................................7** 4.1
> Integración unificada (todo en
> uno)...................................................... 7 4.2
> Integración de elementos
> independientes.............................................13
>
> **5.** **Catálogo** **de**
> **errores............................................................................16**
>
> **6.** **Catálogo** **de**
> **idiomas...........................................................................17**
>
> **7.** **Envío** **de** **operación** **tras** **generación** **de**
> **ID** **de** **operación.....................19**
>
> 6.1 Implementación sin uso de las librerías de ayuda
> ..................................19
>
> **8.** **Identificación** **de** **operaciones** **inSite** **en**
> **el** **Portal** **de** **Administración** **del** **TPV**
> **Virtual**
> **...................................................................................21**
>
> <img src="./kptd0hr4.png"
> style="width:4.61458in;height:4.61458in" /><img src="./nt3jalzq.png" style="width:4.09861in" />**9.**
> **Autenticación** **3DSecure** **en**
> **InSite......................................................21**
>
> 8.1 3DSecure v1.0.2
> ...............................................................................21
> 8.1.1 Solicitar
> autorización....................................................................21
> 8.1.2 Ejecución de la
> autenticación.........................................................23
> 8.1.3 Confirmación de autorización 3DSecure 1.0 posterior al
> Challenge.....24 8.2
> EMV3DS...........................................................................................25
> 8.2.1 Iniciar
> Petición.............................................................................25
> 8.2.2 Ejecución del
> 3DSMethod..............................................................26
> 8.2.3 Petición de autorización con datos
> EMV3DS.....................................28 8.2.4 Ejecución del
> Challenge................................................................30
> 8.2.5 Confirmación de autorización EMV3DS posterior al Challenge
> ............31
>
> Redsys · C/ Francisco Sancha, 12 · 28034 · Madrid · ESPAÑA
>
> Versión: 3.0 \<USO EXTERNO RESTRINGIDO\> 18/04/2022
>
> 2

<img src="./l2yhpndi.png" style="width:4.09861in" />

> <img src="./wjewzvyy.png"
> style="width:1.05208in;height:0.40625in" />***Integración***
> ***inSite***
>
> <img src="./iqhxocvu.png"
> style="width:4.61458in;height:4.61458in" /><img src="./q2dz3aua.png" style="width:4.09861in" />Redsys
> · C/ Francisco Sancha, 12 · 28034 · Madrid · ESPAÑA
>
> Versión: 3.0 \<USO EXTERNO RESTRINGIDO\> 18/04/2022
>
> 3

<img src="./mrpjpd10.png" style="width:4.09861in" />

> <img src="./13mbacu3.png"
> style="width:1.05208in;height:0.40625in" />***Integración***
> ***inSite***
>
> **1.** **Objetivo** **de** **esta** **guía**
>
> En este documento de describe cómo implementar en una tienda web la
> conexión **inSite** del TPV Virtual, un modelo de conexión que permite
> recoger los datos de pago del cliente sin que éste tenga que abandonar
> la página web del comercio.
>
> Las ventajas de este tipo de integración son varias y se describen con
> mayor detalle en el siguiente epígrafe. El objetivo principal es el de
> disponer de un proceso de pago rápido, sencillo e integrado al máximo
> en las páginas de la tienda web, adaptado completamente al diseño del
> comercio online, fácil de usar y de integrar, pero a la vez que
> mantiene la seguridad sobre los datos de pago introducidos por el
> cliente, evitando que el comercio tenga que soportar costosos procesos
> de seguridad derivados del cumplimiento obligatorio de la normativa
> PCI DSS1.
>
> **Esta** **guía** **se** **centra** **en** **las**
> **particularidades** **de** **este** **este** **tipo** **de**
> **integración.** **Paraconceptos** **generales** **del**
> **funcionamiento** **del** **servicio** **de** **TPV** **VirtualSIS**
> **por** **favor** **consulte** **la** **documentación**
> **correspondiente.**
>
> <img src="./jksxmqr1.png"
> style="width:4.61458in;height:4.61458in" /><img src="./05c3mwtb.png" style="width:4.09861in" />1
> PCI DSS (Payment Card Industry Data Security Standard) establece los
> requerimientos de seguridad que los intervinientes en el proceso de
> pago con tarjetas deben cumplir. La solución descrita en el documento
> facilita la consideración del proceso como un tipo SAQ-A, al basar la
> implementación en iframes cuyo contenido sólo es accesible por
> nuestros servidores.
>
> Redsys · C/ Francisco Sancha, 12 · 28034 · Madrid · ESPAÑA
>
> Versión: 3.0 \<USO EXTERNO RESTRINGIDO\> 18/04/2022
>
> 4

<img src="./gtavainq.png" style="width:4.09861in" />

> <img src="./4apyxdo1.png"
> style="width:1.05208in;height:0.40625in" />***Integración***
> ***inSite***
>
> **2.** **Conceptos** **y** **ventajas** **de** **la** **conexión**
> **inSite**
>
> Con la solución de pago ***inSite*** el comercio o tienda online
> consigue una serie de ventajas que favorecen el aumento de la
> conversión de ventas:
>
> • Una **experiencia** **de** **pago** **sencilla** **y**
> **satisfactoria** para sus clientes, al estar **totalmente**
> **integrada** en las páginas web del comercio y sin saltos de
> navegación.
>
> • **Mayor** **control** del flujo de checkout y pago, ya que toda
> petición se realiza de forma síncrona por parte del servidor de la
> tienda web y sin necesidad de procesos asíncronos de “escucha”.
>
> • **Facilidad** **de** **uso** **en** **su** **integración**,
>
> • **Alto** **nivel** **de** **seguridad,** similar a la solución
> basada en redirección del cliente hacia una página de pago externa.
>
> En definitiva, además de un proceso de pago totalmente integrado en el
> checkout al comprador, se permite al comercio una mayor
> **flexibilidad** **y** **control** en el proceso de pago, pudiendo
> además separar los pasos de captura de datos y ejecución de la
> operación.
>
> A la hora de integrar la conexión **inSite** existen **dos**
> **posibilidades**:
>
> • Integración unificada (todo en uno)
>
> • Integración por elementos independientes
>
> <img src="./vay1lb5e.png"
> style="width:4.61458in;height:4.61458in" /><img src="./v4zauywg.png" style="width:4.09861in" />En
> ambos casos, la integración se puede realizar utilizando fragmentos de
> código que se exponen como ejemplos, donde sólo se requiere cambiar
> valores propios como el identificador del comercio o las claves
> utilizadas. Además, como ayuda adicional se proveen librerias
> proporcionadas por Redsys para los principales lenguajes de
> programación.
>
> En la conexión inSite, se facilitan a la tienda online las piezas o
> “campos” necesarios del formulario de pago de forma que se integran
> uno a uno (o como un conjunto) perfectamente incrustados en la página
> checkout de la tienda web y además cada elemento permite
> personalización del diseño con estilos configurables, en perfecta
> sintonía del diseño del resto de la página web del comercio.
>
> La seguridad se preserva de forma que el formulario resultante con la
> información de pago de los clientes queda inaccesible al mismo
> servidor del comercio o incluso de terceros que hayan podido
> comprometer el servidor web del comercio.
>
> Redsys · C/ Francisco Sancha, 12 · 28034 · Madrid · ESPAÑA
>
> Versión: 3.0 \<USO EXTERNO RESTRINGIDO\> 18/04/2022
>
> 5

<img src="./vzj5c3m5.png" style="width:4.09861in" />

> <img src="./esfkmwp4.png"
> style="width:1.05208in;height:0.40625in" />***Integración***
> ***inSite***
>
> **3.** **Descripción** **general** **del** **flujo**
>
> El siguiente esquema presenta el flujo general de una operación
> realizada con el nuevo esquema del TPV Virtual.
>
> <img src="./yam1mpnb.png"
> style="width:4.61458in;height:4.61458in" /><img src="./biefyazz.png" style="width:4.09861in" /><img src="./gpqunapo.png" style="width:6.7in;height:3.64306in" />En
> resumen, los datos de pago introducidos por el cliente son enviados
> desde la página del comercio al TPV Virtual, donde se almacenan
> temporalmente y se asocian a un Id de Operación que se devuelve al
> comercio. Con este Id de Operación (que viene a ser un “alias” de los
> datos de pago del cliente) el comercio puede solicitar posteriormente
> y directamente al tpv virtual la realización de la operación de pago
> deseada.
>
> Redsys · C/ Francisco Sancha, 12 · 28034 · Madrid · ESPAÑA
>
> Versión: 3.0 \<USO EXTERNO RESTRINGIDO\> 18/04/2022
>
> 6

<img src="./tesxoxmz.png" style="width:4.09861in" />

> <img src="./ybijaorv.png"
> style="width:1.05208in;height:0.40625in" />***Integración***
> ***inSite***
>
> **4.** **Página** **de** **Pago** **–** **Obtención** **de** **ID**
> **de** **operación**
>
> Como primer pasopara poderintegrarlos campos deintroduccióndedatos
> detarjeta directamente en su propia página web, se debe incluir el
> fichero Javascript alojado en el servidor de Redsys con la siguiente
> línea de código (el fichero varía según se vaya a usar el entorno de
> test o el entorno de producción real):
>
> \- Entorno de Test:
>
> \<script
> src="https://sis-t.redsys.es:25443/sis/NC/sandbox/redsysV3.js"\>\</script\> -
> Entorno de Producción:
>
> \<script src="https://sis.redsys.es/sis/NC/redsysV3.js"\>\</script\>
>
> El siguiente paso para incluir los elementos del formulario de pago
> depende de la alternativa que se desee implementar. A la hora de
> integrar la conexión **inSite** existen **dos** **posibilidades**:
>
> a\) **Integración** **unificada** **(todo** **en** **uno)**: Los
> elementos de pago, como las cajas de introducción de número de
> tarjeta, fecha de caducidad, cvv.. y botón de pago se incrustan como
> un solo elemento que se adapta a la página del comercio (responsive) ,
> con diseño ligero y estilos CSS personalizables. Incluye por defecto
> ayudas interactivas animadas y una buena usabilidad al usuario.
>
> <img src="./05awpuvo.png"
> style="width:4.61458in;height:4.61458in" /><img src="./yq4njs3u.png" style="width:4.09861in" />b)
> **Integración** **por** **elementos** **independientes**: los campos
> se deben incrustar cada uno de forma independiente dentro de la página
> web de la tienda web, lo que permite el control total del diseño,
> posición, gestión de los errores, etc.
>
> **4.1** **Integración** **unificada** **(todo** **en** **uno)**
>
> En esta modalidad de la integración inSite se proveerá un único iframe
> de tamaño muy ajustado en el que se incluirá el formulario de pago al
> completo. En cuanto a la personalización del mismo, se podrán aplicar
> los estilos CSS que el comercio requiera a los diferentes elementos.
>
> Incluye elementos interactivos que facilitan la usabilidad, como el
> reconocimiento de la marca de tarjeta, mostrando el logo de la misma,
> verificación de los formatos y contenidos y resaltando visualmente
> alguno es incorrecto (check digit, fecha cad…).
>
> Ejemplo:
>
> Redsys · C/ Francisco Sancha, 12 · 28034 · Madrid · ESPAÑA
>
> Versión: 3.0 \<USO EXTERNO RESTRINGIDO\> 18/04/2022
>
> 7

<img src="./tsl2gpav.png" style="width:4.09861in" /><img src="./nxqjh4y3.png"
style="width:4.72083in;height:2.51181in" />

> <img src="./sckjke23.png"
> style="width:1.05208in;height:0.40625in" />***Integración***
> ***inSite***
>
> Una vez importado el fichero JS, se deberá crear el formulario de
> pago. Para recoger de forma segura los datos de tarjeta, Redsys creará
> y alojará los campos de introducción de dichos datos.
>
> Se deberán crear un único contenedor, con un id único, ya que se
> deberá indicar para que se genere iframe con los elementos en él.
>
> \<div id="**card-form-example**"\>\</div\>
>
> <img src="./sfyeg4tb.png"
> style="width:4.61458in;height:4.61458in" /><img src="./cb5bugzg.png" style="width:4.09861in" />Se
> debe incluir una función de escucha de mensajes (listener) para
> recibir el ID de operación cuando éste se genere. Se debe utilizar la
> función *storeIdOper* con la siguiente definición:
>
> *storeIdOper(event,* *idToken,* *idErrorMsg,* *validationFunction);*
>
> en la que se deberá indicar el evento recogido por el listener
> (*event*), el ID del elemento del DOM se debe almacenar el ID de
> operación una vez sea generado (*idToken*), el identificador del
> elemento en elque sealmacenarán los códigos de error en caso de que
> existan errores de validación de los datos (*idErrorMsg*). En el
> ejemplo posterior ambos se almacenan en un input de tipo “hidden”.
>
> Opcionalmente, se establece la posibilidad de ejecutar una función
> propia para realizar validaciones previas por parte del comercio.
> Únicamente se continuará con la generación del ID de operación si la
> función de validación ejecutada por el comercio retorna un valor
> *true*.
>
> Redsys · C/ Francisco Sancha, 12 · 28034 · Madrid · ESPAÑA
>
> Versión: 3.0 \<USO EXTERNO RESTRINGIDO\> 18/04/2022
>
> 8

<img src="./pv02adjc.png" style="width:4.09861in" />

> <img src="./xdk4lsn1.png"
> style="width:1.05208in;height:0.40625in" />***Integración***
> ***inSite***
>
> \<input type="hidden" id="**token**" \>\</input\> \<input
> type="hidden" id="**errorCode**" \>\</input\> \<script\>
>
> function **merchantValidationEjemplo**(){ //Insertar validaciones…
>
> return true; }
>
> \<!-- Listener --\>
>
> window.addEventListener("message", function receiveMessage(**event**)
> { storeIdOper(**event**,"**token**", “**errorCode**”,
> **merchantValidationEjemplo**);
>
> }); \</script\>
>
> Una vez preparado el listener para la recepción de los datos, se
> llamará a la función proporcionada para generar los elementos de
> introducción de datos de tarjeta. Hay disponibles dos funciones,
> pudiendo usar la que prefiramos. En la primera pasaremos cada dato en
> un parámetro (getInSiteForm()), y en la segunda pasaremos los datos en
> formato JSON(getInSiteFormJSON()). La ventaja de esta última es que
> podremos pasar solo los datos que necesitemos, sin necesidad de enviar
> parámetros vacíos.:
>
> <img src="./udbeaffj.png"
> style="width:4.61458in;height:4.61458in" /><img src="./xnm2yfzo.png" style="width:4.09861in" />\<!--
> Petición de carga de iframe clásica--\>
>
> getInSiteForm(idContenedor, estiloBoton, estiloBody, estiloCaja,
> estiloInputs, buttonValue, fuc, terminal, merchantOrder, idiomaInsite,
> mostrarLogo, estiloReducido, estiloInsite);
>
> \<!-- Ejemplo --\>
>
> getInSiteForm('card-form', '', '', '', '', 'Texto botón pago',
> ‘123456789', '1', ‘ped4227’, 'ES', true, false, 'twoRows');
>
> \<!-- Petición de carga de iframe JSON--\> var insiteJSON = {
>
> "id" : "card-form", "fuc" : "123456789", "terminal" : "1", "order" :
> “ped4227”, "estiloInsite" : "inline"
>
> }
>
> Redsys · C/ Francisco Sancha, 12 · 28034 · Madrid · ESPAÑA
>
> Versión: 3.0 \<USO EXTERNO RESTRINGIDO\> 18/04/2022
>
> 9

<img src="./ygkuxnpx.png" style="width:4.09861in" />

> <img src="./tcouvdvl.png"
> style="width:1.05208in;height:0.40625in" />***Integración***
> ***inSite***
>
> getInSiteFormJSON(insiteJSON);
>
> Como parámetros de las funciones se indicará el id del contenedor
> reservado para su generación, asícomoel estilo requeridopara los
> diferentes elementos (formatoCSS). En esta modalidad, se podrán
> incluir estilos para diferentes elementos:
>
> • *<u>Botón de pago</u>* →Se permite la personalización completa del
> botón de pago. • *<u>Cuerpo del formulario</u>* →Se recomienda
> utilizar para establecer un color de
>
> fondo o modificar el color o estilo de los textos.
>
> • *<u>Caja de introducción de datos</u>* →Se podrá establecer un color
> de fondo diferenciado para la caja de introducción de datos. El color
> del texto aplicado en este elemento se aplicará al “placeholder” de
> los elementos.
>
> • *<u>Inputs de introducción de datos</u>* →Se recomienda su uso si se
> quiere utilizar un tipo de letra diferente o modificar el color del
> texto de los campos de introducción de datos.
>
> Adicionalmente, se podrá personalizar el texto a incluir en el botón
> y, por último, se deberá informar el valor del FUC, terminal y número
> de pedido (cadena de texto alfanumérica de entre 4 y 12 posiciones) en
> la petición de carga del iframe con el formulario de pago.
>
> Se incluyen cuatro parámetros opcionales en la carga del iframe. Los
> parámetros opcionales son los siguientes (en orden):
>
> \- **Idioma:** Indica el idioma de los textos. La relación de códigos
> se puede encontrar en el apartado <u>Catálogo de idiomas</u>.
>
> Se puede usar tanto el código SIS como el código ISO 639-1 del idioma.
>
> <img src="./rudfxxaw.png"
> style="width:4.61458in;height:4.61458in" /><img src="./scjgawmm.png" style="width:4.09861in" />En
> caso de no establecerse ningún idioma o informar un código incorrecto,
> el idioma por defecto será el Castellano.
>
> \- **Logo** **entidad.** Establece si se quiere mostrar el logo de la
> entidad, indicando **true** si se desea mostrarlo o **false** si en el
> caso contrario.
>
> En caso de no establecer ningún valor, por defecto se mostrará el logo
> de la entidad.
>
> \- **Estilo** **reducido**. Indica si se quiere mostrar Insite con un
> ancho reducido. - **Estilo** **de** **Insite.** Se puede elegir entre
> dos estilos predefinidos de Insite,
>
> **‘inline’** o **‘twoRows'**. Por defecto se pintará con el estilo
> ‘inline’. A continuación, se puede ver un ejemplo de cada estilo de
> Insite con sus respectivas versiones reducidas:
>
> Redsys · C/ Francisco Sancha, 12 · 28034 · Madrid · ESPAÑA
>
> Versión: 3.0 \<USO EXTERNO RESTRINGIDO\> 18/04/2022
>
> 10

<img src="./d1htkvrh.png" style="width:4.09861in" /><img src="./rvmxpswn.png"
style="width:2.65306in;height:1.11597in" /><img src="./e20g0hgc.png"
style="width:2.02181in;height:1.19722in" /><img src="./hndfgy00.png"
style="width:2.29153in;height:1.65347in" /><img src="./v3xb32s4.png"
style="width:1.82431in;height:1.68319in" />

> <img src="./sjryji20.png"
> style="width:1.05208in;height:0.40625in" />***Integración***
> ***inSite***
>
> <u>Inline</u>
>
> <u>TwoRows</u>
>
> <u>Inline reducido</u>

<u>TwoRows reducido</u>

> Puede consultar la lista de parámetros para enviar los datos en
> formato JSON a continuación:
>
> <img src="./eydizd4q.png"
> style="width:4.61458in;height:4.61458in" /><img src="./j4ev32rp.png" style="width:4.09861in" />Redsys
> · C/ Francisco Sancha, 12 · 28034 · Madrid · ESPAÑA
>
> Versión: 3.0 \<USO EXTERNO RESTRINGIDO\> 18/04/2022
>
> 11

<img src="./uqtqykwo.png" style="width:4.09861in" />

> <img src="./iarocfud.png"
> style="width:1.05208in;height:0.40625in" />***Integración***
> ***inSite***
>
> **\*El** **merchantOrder** **utilizado** **en** **la** **carga**
> **del** **iframe** **y** **generación** **del** **idOper** **deberá**
> **reutilizarse** **en** **la** **posterior** **petición** **de**
> **autorización.**
>
> De esta forma, cuando el cliente introduzca sus datos de tarjeta en
> los elementos generados por Redsys y pulse el botón de pago, se
> generará y almacenará en el formulario del comercio un ID asociado a
> la operación para que éste formalice la compra sin necesidad de tratar
> datos de tarjeta.
>
> <img src="./pkrhjd1o.png"
> style="width:4.61458in;height:4.61458in" /><img src="./s2fqrqxo.png" style="width:4.09861in" />Redsys
> · C/ Francisco Sancha, 12 · 28034 · Madrid · ESPAÑA
>
> Versión: 3.0 \<USO EXTERNO RESTRINGIDO\> 18/04/2022
>
> 12

<img src="./vsep1uf2.png" style="width:4.09861in" />

> <img src="./vj54ytk4.png"
> style="width:1.05208in;height:0.40625in" />***Integración***
> ***inSite***
>
> **4.2** **Integración** **de** **elementos** **independientes**
>
> En esta modalidad de la integración inSite se permitirá a los
> comercios una total personalización de la página de pago, por lo que
> podrá colocar los campos de introducción de datos de tarjeta y el
> botón de pago con total libertad, al generar iframes diferenciados y
> personalizables con estilos para cada uno de ellos.
>
> También se incluyen elementos para mejorar la usabilidad como, por
> ejemplo, el reconocimiento de la marca de tarjeta mostrando el icono
> de ésta, longitud del CVV teniendo en cuenta la marca de la tarjeta o
> el formato de fecha de caducidad.
>
> Una vez importado el fichero, se deberá crear el formulario de pago.
> Para recoger de forma segura los datos detarjeta, Redsys creará y
> alojará los campos deintroducción de dichos datos.
>
> Se deberán crear contenedores vacíos, con un id único, ya que se
> deberá indicarpara que se genere el campo de introducción de datos en
> él.
>
> \<div class="cardinfo-card-number"\>
>
> \<label class="cardinfo-label" for="card-number"\>Numero de
> tarjeta\</label\> \<div class='input-wrapper'
> id="**card-number**"\>\</div\>
>
> \</div\>
>
> \<div class="cardinfo-exp-date"\>
>
> \<label class="cardinfo-label" for="expiration-month"\>Mes Caducidad
> (MM)\</label\> \<div class='input-wrapper'
> id="**expiration-month**"\>\</div\>
>
> <img src="./xxpkiefe.png"
> style="width:4.61458in;height:4.61458in" /><img src="./vkvhz34z.png" style="width:4.09861in" />\</div\>
>
> \<div class="cardinfo-exp-date2"\>
>
> \<label class="cardinfo-label" for="expiration-year"\>Ano Caducidad
> (AA)\</label\> \<div class='input-wrapper'
> id="**expiration-year**"\>\</div\>
>
> \</div\>
>
> \<div class="cardinfo-cvv"\>
>
> \<label class="cardinfo-label" for="cvv"\>CVV\</label\> \<div
> class='input-wrapper' id="**cvv**"\>\</div\>
>
> \</div\>
>
> \<div id="**boton**"\>\</div\>
>
> En este ejemplo se utilizan elementos de fecha independientes, uno
> para mes (”expiration-month”) y otro para año (”expiration-year”).
>
> Si se desea mostrar el mes y el año correspondientes a la fecha de
> caducidad en el mismo campo está disponible un elemento que incluye
> ambos valores en formato mm/aa.
>
> Redsys · C/ Francisco Sancha, 12 · 28034 · Madrid · ESPAÑA
>
> Versión: 3.0 \<USO EXTERNO RESTRINGIDO\> 18/04/2022
>
> 13

<img src="./hmjhqarg.png" style="width:4.09861in" />

> <img src="./0edohtli.png"
> style="width:1.05208in;height:0.40625in" />***Integración***
> ***inSite***
>
> Para ello reemplazaremos los elementos ”*expiration-month*” y
> ”*expiration-year*” por el elemento *“card-expiration”.*
>
> \<div class="cardinfo-card-number"\>
>
> \<label class="cardinfo-label" for="card-number"\>Numero de
> tarjeta\</label\> \<div class='input-wrapper'
> id="card-number"\>\</div\>
>
> \</div\>
>
> **\<div** **class="cardinfo-exp-date"\>**
>
> **\<label** **class="cardinfo-label"**
> **for="card-expiration"\>Caducidad\</label\>** **\<div**
> **class='input-wrapper'** **id="card-expiration"\>\</div\>**
>
> **\</div**\>
>
> \<div class="cardinfo-cvv"\>
>
> \<label class="cardinfo-label" for="cvv"\>CVV\</label\> \<div
> class='input-wrapper' id="cvv"\>\</div\>
>
> \</div\>
>
> \<div id="boton"\>\</div\>
>
> Se debe incluir una función de escucha de mensajes (listener) para
> recibir el ID de operación cuando éste se genere. Se debe utilizar la
> función *storeIdOper* con la siguiente definición:
>
> *storeIdOper(event,* *idToken,* *idErrorMsg,* *validationFunction);*
>
> <img src="./m5jc3a1n.png"
> style="width:4.61458in;height:4.61458in" /><img src="./mbzt2ebr.png" style="width:4.09861in" />en
> la que se deberá indicar el evento recogido por el listener (*event*),
> el ID del elemento del DOM se debe almacenar el ID de operación una
> vez sea generado (*idToken*), el identificador del elemento en elque
> sealmacenarán los códigos de error en caso de que existan errores de
> validación de los datos (*idErrorMsg*). En el ejemplo posterior ambos
> se almacenan en un input de tipo “hidden”.
>
> Opcionalmente, se establece la posibilidad de ejecutar una función
> propia para realizar validaciones previas por parte del comercio.
> Únicamente se continuará con la generación del ID de operación si la
> función de validación ejecutada por el comercio retorna un valor
> *true*.
>
> \<input type="hidden" id="**token**" \>\</input\> \<input
> type="hidden" id="**errorCode**" \>\</input\> \<script\>
>
> function **merchantValidationEjemplo**(){ //Insertar validaciones…
>
> return true; }
>
> Redsys · C/ Francisco Sancha, 12 · 28034 · Madrid · ESPAÑA
>
> Versión: 3.0 \<USO EXTERNO RESTRINGIDO\> 18/04/2022
>
> 14

<img src="./yjbymnv1.png" style="width:4.09861in" />

> <img src="./wgkyvppi.png"
> style="width:1.05208in;height:0.40625in" />***Integración***
> ***inSite***
>
> \<!-- Listener --\>
>
> window.addEventListener("message", function receiveMessage(**event**)
> { storeIdOper(**event**,"**token**", “**errorCode**”,
> **merchantValidationEjemplo**);
>
> }); \</script\>
>
> Una vez preparado el envío de datos y la posterior recepción, se
> llamará a las funciones proporcionadas para generar los elementos de
> introducción de datos de tarjeta:
>
> \<!-- Petición de carga de iframes --\>
>
> getCardInput('card-number', estiloCaja, placeholder, estiloInput);
> getExpirationMonthInput('expiration-month', estilosCSS, placeholder);
> getExpirationYearInput('expiration-year', estilosCSS, placeholder);
> getCVVInput('cvv', estilosCSS, placeholder);
>
> getPayButton('boton', estilosCSS, 'Pagar con Redsys', fuc, terminal,
> merchantOrder);
>
> Si se utiliza el elemento de fecha unificado (mm/aa) reemplazaremos
> las funciones getExpirationMonthInput() y getExpirationYearInput() por
> la función getExpirationInput().
>
> // Petición de carga de iframes
>
> <img src="./u4uhbwf0.png"
> style="width:4.61458in;height:4.61458in" /><img src="./gtgwbjx1.png" style="width:4.09861in" />getCardInput('card-number',
> estiloCaja, placeholder, estiloInput);
> **getExpirationInput('card-expiration',** **estilosCSS,**
> **placeholder);** getCVVInput('cvv', estilosCSS, placeholder);
>
> getPayButton('boton', estilosCSS, 'Pagar con Redsys', fuc, terminal,
> merchantOrder);
>
> Como parámetros de las funciones se indicará el id del contenedor
> reservado para su generación, el estilo requerido para el mismo
> (formato CSS) y el placeholder de dicho campo. En el caso de la
> función getCardInput() sepasarán dos campos de estilo CSS, uno con el
> estilo de la caja exterior y otro con el estilo del input que
> contiene. Adicionalmente, se podrá personalizar el texto del botón de
> pago y, por último, se deberá informarelvalor delFUC,terminal y
> númerodepedido (alfanuméricodeentre 4 y 12 posiciones) en la petición
> de carga del iframe con el botón de pago.
>
> **El** **merchantOrder** **utilizado** **en** **la** **generación**
> **del** **idOper** **deberá** **reutilizarse** **en** **la**
> **posterior** **petición** **de** **autorización.**
>
> De esta forma, cuando el cliente introduzca sus datos de tarjeta en
> los elementos generados por Redsys y pulse el botón de pago, se
> generará y almacenará en el formulario del comercio un ID asociado a
> la operación para que éste formalice la compra sin necesidad de tratar
> datos de tarjeta.
>
> Redsys · C/ Francisco Sancha, 12 · 28034 · Madrid · ESPAÑA
>
> Versión: 3.0 \<USO EXTERNO RESTRINGIDO\> 18/04/2022
>
> 15

<img src="./505q5qwp.png" style="width:4.09861in" />

> <img src="./m0wdy04k.png"
> style="width:1.05208in;height:0.40625in" />***Integración***
> ***inSite***
>
> **5.** **Catálogo** **de** **errores**
>
> Cuando se pulse el botón generado por Redsys se lanzarán las
> validaciones a partir de los datos introducidos. Se podrá recibir el
> siguiente catálogo de errores.
>
> Se incluye la descripción de los errores, pero es responsabilidad del
> comercio mostrarlos de la forma que considere adecuada.
>
> <img src="./maib0lsa.png"
> style="width:4.61458in;height:4.61458in" /><img src="./0scvilo3.png" style="width:4.09861in" />Redsys
> · C/ Francisco Sancha, 12 · 28034 · Madrid · ESPAÑA
>
> Versión: 3.0 \<USO EXTERNO RESTRINGIDO\> 18/04/2022
>
> 16

<img src="./smmctwm3.png" style="width:4.09861in" />

> <img src="./witqeg5n.png"
> style="width:1.05208in;height:0.40625in" />***Integración***
> ***inSite***
>
> **6.** **Catálogo** **de** **idiomas**
>
> <img src="./5qzxgqz1.png"
> style="width:4.61458in;height:4.61458in" /><img src="./x2r440yx.png" style="width:4.09861in" />Redsys
> · C/ Francisco Sancha, 12 · 28034 · Madrid · ESPAÑA
>
> Versión: 3.0 \<USO EXTERNO RESTRINGIDO\> 18/04/2022
>
> 17

<img src="./g5hkoybl.png" style="width:4.09861in" />

> <img src="./2tnkasil.png"
> style="width:1.05208in;height:0.40625in" />***Integración***
> ***inSite***

||
||
||
||
||
||

> <img src="./zt35s1cy.png"
> style="width:4.61458in;height:4.61458in" /><img src="./aeuk3ox2.png" style="width:4.09861in" />Redsys
> · C/ Francisco Sancha, 12 · 28034 · Madrid · ESPAÑA
>
> Versión: 3.0 \<USO EXTERNO RESTRINGIDO\> 18/04/2022
>
> 18

<img src="./ar1styzl.png" style="width:4.09861in" />

> <img src="./p1hvlns4.png"
> style="width:1.05208in;height:0.40625in" />***Integración***
> ***inSite***
>
> **7.** **Envío** **de** **operación** **tras** **generación** **de**
> **ID** **de** **operación**
>
> Una vez recibido y almacenado el ID de operación por parte del
> comercio según se ha descrito en los apartados anteriores, podrá
> lanzar la operación de autorización utilizando cualquiera de los
> interfaces disponibles en el TPV Virtual.
>
> **En** **la** **operación** **de** **autorización,** **se** **tendrá**
> **que** **enviar** **el** **parámetro** **DS_MERCHANT_IDOPER** **en**
> **lugar** **de** **los** **campos** **habituales** **de** **envío**
> **de** **datos** **de** **tarjeta.** **Además,** **se** **deberá**
> **utilizar** **el** **mismo** **número** **de** **pedido**
> **(DS_MERCHANT_ORDER)** **que** **el** **utilizado** **en** **la**
> **generación** **del** **idOper.**
>
> Se pone a disposición de los comercios librerías que simplifican esta
> conexión en lenguajes Java y PHP. Su descarga está disponible en la
> sección de descargas de la web de desarrolladores de Redsys:
>
> [<u>https://pagosonline.redsys.es/descargas.html</u>](https://pagosonline.redsys.es/descargas.html)
>
> La descarga de las librerías incluye documentación de ayuda para su
> uso.
>
> **6.1** **Implementación** **sin** **uso** **de** **las**
> **librerías** **de** **ayuda**
>
> Si no se desea utilizar las librerías de ayuda o se quiere implementar
> para otros lenguajes de programación, pueden implementar directamente
> la llamada REST al TPV Virtual. **Para** **obtener** **más**
> **información**, se recomienda consultar la documentación de
> Integración vía REST.
>
> La solicitud de autorización se hace a través de una petición al TPV
> Virtual En dicha petición deberá incluir los siguientes parámetros:
>
> <img src="./3ze1r24p.png"
> style="width:4.61458in;height:4.61458in" /><img src="./bpsvidnl.png" style="width:4.09861in" />•
> Ds_SignatureVersion: Constante que indica la versión de firma que se
> está utilizando.
>
> • Ds_MerchantParameters: Cadena en formato JSON con todos los
> parámetros de la petición codificada en Base 64 y sin retornos de
> carro (Consultar [Parámetros de entrada y
> salida)](file://///smp2020.redsys.local/AEA/NUEVOS%20CANALES/TPVVIRTUAL/DOCUMENTACION_NUEVA/GuÃ­a%20IntegraciÃ³n/Manual%20IntegraciÃ³n%20-%20Rest/TPV-Virtual%20Manual%20IntegraciÃ³n%20-%20Rest%20V2.4%20-%20(en%20vigor%20-%20NO%20MODIFICAR%20-%20enviada%20a%20entidades%2012-11-2019)%20.docx%23_ParÃ¡metros_de_entrada).
>
> • Ds_Signature: Firma de los datos enviados. Es el resultado del HMAC
> SHA256 de la cadena JSON codificada en Base 64 enviada en el parámetro
> anterior
>
> Dichos parámetros deben enviarse a los siguientes endpoints
> dependiendo de si se quiere realizar una petición en el entorno de
> prueba u operaciones reales:
>
> Redsys · C/ Francisco Sancha, 12 · 28034 · Madrid · ESPAÑA
>
> Versión: 3.0 \<USO EXTERNO RESTRINGIDO\> 18/04/2022
>
> 19

<img src="./oaiekhni.png" style="width:4.09861in" />

> <img src="./wujjkand.png"
> style="width:1.05208in;height:0.40625in" />***Integración***
> ***inSite***
>
> Una vez gestionada la consulta, el TPV Virtual informará al servidor
> del comercio el resultado de esta con la información del resultado
> incluida en un fichero JSON. En él se incluirán los siguientes campos:
>
> • Ds_SignatureVersion: Constante que indica la versión de firma que se
> está utilizando.
>
> • Ds_MerchantParameters: Cadena en formato JSON con todos los
> parámetros de la respuesta codificada en Base 64 y sin retornos de
> carro.
>
> • Ds_Signature: Firma de los datos enviados. Resultado del HMAC SHA256
> de la cadena JSON codificada en Base 64 enviada en el parámetro
> anterior**.** **El** **comercio** **es** **responsable** **de**
> **validar** **el** **HMAC** **enviado** **por** **el** **TPV**
> **Virtual** **para** **asegurarse** **de** **la** **validez** **de**
> **la** **respuesta.** **Esta** **validación** **es** **necesaria**
> **para** **garantizar** **que** **los** **datos** **no** **han**
> **sido** **manipulados** **y** **que** **el** **origen** **es**
> **realmente** **el** **TPV** **Virtual.**
>
> A continuación, se describen los datos que debe incluir el
> Ds_MerchantParameters para enviar una petición de autenticación al
> Servicio REST:
>
> {
>
> "DS_MERCHANT_ORDER":1552572812,
> "DS_MERCHANT_MERCHANTCODE":"999008881", "DS_MERCHANT_TERMINAL":"2",
> "DS_MERCHANT_CURRENCY":"978", "DS_MERCHANT_TRANSACTIONTYPE":"0",
> "DS_MERCHANT_AMOUNT":"1000",
>
> **"DS_MERCHANT_IDOPER":"**
> **455097a74c21b761be86acb26c32609dce222e66",** }
>
> Como respuesta se obtendrá:
>
> {
>
> <img src="./sdqt4sei.png"
> style="width:4.61458in;height:4.61458in" /><img src="./ezts0wwa.png" style="width:4.09861in" />"Ds_Amount":"1000",
> "Ds_Currency":"978", "Ds_Order":"1552572812",
> "Ds_MerchantCode":"999008881", "Ds_Terminal":"2",
> "Ds_Response":"0000", "Ds_AuthorisationCode":"694432",
> "Ds_TransactionType":"0", "Ds_SecurePayment":"1", "Ds_Language":"1",
> "Ds_CardNumber":"454881\*\*\*\*\*\*0004", "Ds_Card_Type":"C",
> "Ds_MerchantData":"", "Ds_Card_Country":"724", "Ds_Card_Brand":"1"
>
> }
>
> Redsys · C/ Francisco Sancha, 12 · 28034 · Madrid · ESPAÑA
>
> Versión: 3.0 \<USO EXTERNO RESTRINGIDO\> 18/04/2022
>
> 20

<img src="./dnqpuvqh.png" style="width:4.09861in" />

> <img src="./0ocgtloo.png"
> style="width:1.05208in;height:0.40625in" />***Integración***
> ***inSite***
>
> **8.** **Identificación** **de** **operaciones** **inSite** **en**
> **el** **Portal** **de** **Administración** **del** **TPV**
> **Virtual**

En el portal de administración del TPV Virtual se puede identificar las
operaciones realizadas con inSite consultando el campo “entrada” de la
consulta de operaciones.

> Las operaciones se registrarán con la entrada “inSite REST”
>
> **9.** **Autenticación** **3DSecure** **en** **InSite**
>
> Los comercios que utilicen la conexión inSite tienen la posibilidad de
> incluir el protocolo 3DSecure (3DS) para autenticar a los titulares y
> obtener un nivel adicional de protección ante fraude.
>
> Incluirla autenticación3DS implica redirigirla
> navegacióndelclientehacia elservidor deautenticacióndelbanco/entidad
> emisora dela tarjeta para queéstepueda solicitar las credenciales
> necesarias. Este paso debe realizarse en un paso posterior al de
> recoger los datos de tarjeta descrito en los apartados anteriores.
>
> Para utilizar la autenticación 3DS, el terminal del TPV Virtual debe
> estar configurado por parte de la entidad financiera para soportar
> autenticación 3D Secure. Igualmente podría ser necesario que por
> configuración del TPV Virtual este paso no solo sea opcional, sino que
> sea requerido para la correcta autorización de las operaciones (si
> tiene dudas consulte la configuración 3DS a su entidad financiera
> proveedora del TPV Virtual).
>
> **8.1** **3DSecure** **v1.0.2**
>
> <img src="./k314mytn.png"
> style="width:4.61458in;height:4.61458in" /><img src="./wkt0ezsy.png" style="width:4.09861in" />**8.1.1**
> **Solicitar** **autorización**
>
> La solicitud de autorización se hace a través de una petición al TPV
> Virtual En dicha petición deberá incluir los siguientes parámetros:
>
> • Ds_SignatureVersion: Constante que indica la versión de firma que se
> está utilizando.
>
> • Ds_MerchantParameters: Cadena en formato JSON con todos los
> parámetros de la petición codificada en Base 64 y sin retornos de
> carro (Consultar [Parámetros de entrada y
> salida)](file://///smp2020.redsys.local/AEA/NUEVOS%20CANALES/TPVVIRTUAL/DOCUMENTACION_NUEVA/GuÃ­a%20IntegraciÃ³n/Manual%20IntegraciÃ³n%20-%20Rest/TPV-Virtual%20Manual%20IntegraciÃ³n%20-%20Rest%20V2.4%20-%20(en%20vigor%20-%20NO%20MODIFICAR%20-%20enviada%20a%20entidades%2012-11-2019)%20.docx%23_ParÃ¡metros_de_entrada).
>
> • Ds_Signature: Firma de los datos enviados. Es el resultado del HMAC
> SHA256 de la cadena JSON codificada en Base 64 enviada en el parámetro
> anterior
>
> Redsys · C/ Francisco Sancha, 12 · 28034 · Madrid · ESPAÑA
>
> Versión: 3.0 \<USO EXTERNO RESTRINGIDO\> 18/04/2022
>
> 21

<img src="./ym3watws.png" style="width:4.09861in" />

> <img src="./jp04mysb.png"
> style="width:1.05208in;height:0.40625in" />***Integración***
> ***inSite***
>
> Dichos parámetros deben enviarse a los siguientes endpoints
> dependiendo de si se quiere realizar una petición en el entorno de
> prueba u operaciones reales:

||
||
||
||
||

> Una vez gestionada la consulta, el TPV Virtual informará al servidor
> del comercio el resultado de esta, con la información del resultado
> incluida en un fichero JSON. En él se incluirán los siguientes campos:
>
> • Ds_SignatureVersion: Constante que indica la versión de firma que se
> está utilizando.
>
> • Ds_MerchantParameters: Cadena en formato JSON con todos los
> parámetros de la respuesta codificada en Base 64 y sin retornos de
> carro.
>
> • Ds_Signature: Firma de los datos enviados. Resultado del HMAC SHA256
> de la cadena JSON codificada en Base 64 enviada en el parámetro
> anterior**.** **El** **comercio** **es** **responsable** **de**
> **validar** **el** **HMAC** **enviado** **por** **el** **TPV**
> **Virtual** **para** **asegurarse** **de** **la** **validez** **de**
> **la** **respuesta.** **Esta** **validación** **es** **necesaria**
> **para** **garantizar** **que** **los** **datos** **no** **han**
> **sido** **manipulados** **y** **que** **el** **origen** **es**
> **realmente** **el** **TPV** **Virtual.**
>
> A continuación, se describen los datos que debe incluir el
> Ds_MerchantParameters para enviar una petición de autenticación al
> Servicio REST:
>
> <img src="./3e0s2vyr.png"
> style="width:4.61458in;height:4.61458in" /><img src="./0xkesqja.png" style="width:4.09861in" />{
>
> "DS_MERCHANT_ORDER":1552642885,
> "DS_MERCHANT_MERCHANTCODE":"999008881", "DS_MERCHANT_TERMINAL":"2",
> "DS_MERCHANT_CURRENCY":"978", "DS_MERCHANT_TRANSACTIONTYPE":"0",
> "DS_MERCHANT_AMOUNT":"1000",
>
> **"DS_MERCHANT_IDOPER":"**
> **455097a74c21b761be86acb26c32609dce222e66",**
> **"DS_MERCHANT_EMV3DS":{**
>
> **"threeDSInfo":"AuthenticationData",** **"protocolVersion":"1.0.2",**
>
> **"browserAcceptHeader":"text/html,application/xhtml+xml,application/xml;q=0.9,\*/\*;q=0**
> **.8,application/json",**
>
> **"browserUserAgent":"Mozilla/5.0** **(WindowsNT** **10.0;**
> **Win64;** **x64)** **AppleWebKit/537.36** **(KHTML,** **like**
> **Gecko)** **Chrome/71.0.3578.98** **Safari/537.36"**
>
> **}** }
>
> Redsys · C/ Francisco Sancha, 12 · 28034 · Madrid · ESPAÑA
>
> Versión: 3.0 \<USO EXTERNO RESTRINGIDO\> 18/04/2022
>
> 22

<img src="./vmkntvcy.png" style="width:4.09861in" />

> <img src="./wg5rtof4.png"
> style="width:1.05208in;height:0.40625in" />***Integración***
> ***inSite***
>
> Como respuesta se obtendrá lo siguiente:
>
> {
>
> "Ds_Order":"1552642885", "Ds_MerchantCode":"999008881",
> "Ds_Terminal":"2", "Ds_Currency":"978", "Ds_Amount":"1000",
> "Ds_TransactionType":"0", **"Ds_EMV3DS":** **{**
>
> **"threeDSInfo":"ChallengeRequest",** **"protocolVersion":"1.0.2",**
>
> **"acsURL":**
> **"https://sis.redsys.es/sis-simulador-web/authenticationRequest.jsp",**
> **"PAReq":"eJxVUttygjAQ/RWG95KEooKzpkPVjj7QOpZ+QBp2KlYuDVDx77tRqS0zmdmzJ+zlnMBDXxycbzR**
> **NXpUzV3jcdbDUVZaXHzP3LX26C90HCenOIC5eUXcGJSTYNOoDnTybuU1Rq7zPsFGlFmIU+mOfi4j7vrAfn**
> **3DOw9HYlbCJt/gl4dpKUifPBzZAqmn0TpWtBKW/HtfPMhgFYSiAXSEUaNYL+brcJstNnCy381X8nAK7pKF**
> **UBcp5RUjnlZOhU5sO35XzZFSXIbAzD7rqytac5MQPgA0AOnOQu7atp4wdj0fPYNacGk9XBTBLAbvNtuls1F**
> **CpPs9kso/7lzQ+Jftln6R09p88WcRHOjNg9gZkqkU5KOIIPhVi6kfAznlQhZ1BintPcNr0gqC2TeKBsszfDJAHhi**
> **w6yWgS0hYDAuzrqkS6QbL+xkDOaEY73Cafr6zGuiXZ/lololEYWLnPjK2WkzhBJC7lLABm/2VXJ9n1GVD07**
> **3n8AOa7wW0=",**
>
> **"MD":"cd164a6d0b77c96f7ef476121acfa987a0edf602"** **}**
>
> }
>
> **8.1.2** **Ejecución** **de** **la** **autenticación**
>
> El comercio deberá montar un formulario que envíe un POST a la URL del
> parámetro acsURL obtenido en la respuesta de la petición de
> autorización anterior. Dicho formulario envía 3 parámetros necesarios
> para la autenticación:
>
> • *PaReq*, cuyo valor se obtiene del parámetro PAReq obtenido en la
> respuesta de la petición de autorización anterior.
>
> <img src="./irpzvosu.png"
> style="width:4.61458in;height:4.61458in" /><img src="./1lxzegtw.png" style="width:4.09861in" />•
> *MD*, cuyo valor se obtiene del parámetro MD obtenido en la respuesta
> de la petición de autorización anterior.
>
> • *TermUrl*, que identifica la URL a la que entidad Emisora hará un
> POST con el resultado de autenticación. Dicho formulario enviará un
> único parámetro *PARes*, que contiene el resultado de la autenticación
> y que deberá ser recogido por el comercio para su posterior envío en
> la petición de confirmación de autorización.
>
> Redsys · C/ Francisco Sancha, 12 · 28034 · Madrid · ESPAÑA
>
> Versión: 3.0 \<USO EXTERNO RESTRINGIDO\> 18/04/2022
>
> 23

<img src="./b1iwkwg4.png" style="width:4.09861in" />

> <img src="./nkljlsgy.png"
> style="width:1.05208in;height:0.40625in" />***Integración***
> ***inSite***
>
> **8.1.3** **Confirmación** **de** **autorización** **3DSecure**
> **1.0** **posterior** **al** **Challenge**
>
> A continuación, se describen los datos que debe incluir el
> Ds_MerchantParameters para enviar una petición de confirmación de
> autorización 3DSecure 1.0 al Servicio REST:
>
> {
>
> "DS_MERCHANT_ORDER":1552642885,
> "DS_MERCHANT_MERCHANTCODE":"999008881", "DS_MERCHANT_TERMINAL":"2",
> "DS_MERCHANT_CURRENCY":"978", "DS_MERCHANT_TRANSACTIONTYPE":"0",
> "DS_MERCHANT_AMOUNT":"1000", **"DS_MERCHANT_EMV3DS":{**
>
> **"threeDSInfo":"ChallengeResponse",** **"protocolVersion":"1.0.2",**
>
> **"PARes":"eJzFWNmSo0iyfecrymoeNVVsWqBNmWPBKlaJVcAbmwBJLALE9vWDlJVZWT3VNn3v**
> **w70yyRR4uDvu**
> **ESeOu8X2X0N+/dLFdZOVxctX9Dvy9UtchGWUFcnLV8vkvhFf//W6NdM6jhkjDu91/LpV4qbxk/hL**
> **…..",**
>
> **"MD":"035535127d549298f11d7d2fc1b0d4e9300f93f1"** **}**
>
> }
>
> Como respuesta se obtendrá el resultado final de la operación:
>
> {
>
> <img src="./ekdt01v2.png"
> style="width:4.61458in;height:4.61458in" /><img src="./gi4nbbgd.png" style="width:4.09861in" />"Ds_Amount":"1000",
> "Ds_Currency":"978", "Ds_Order":"1552642885",
> "Ds_MerchantCode":"999008881", "Ds_Terminal":"2",
> "Ds_Response":"0000", "Ds_AuthorisationCode":"694432",
> "Ds_TransactionType":"0", "Ds_SecurePayment":"1", "Ds_Language":"1",
> "Ds_CardNumber":"454881\*\*\*\*\*\*0004", "Ds_Card_Type":"C",
> "Ds_MerchantData":"", "Ds_Card_Country":"724", "Ds_Card_Brand":"1"
>
> }
>
> Redsys · C/ Francisco Sancha, 12 · 28034 · Madrid · ESPAÑA
>
> Versión: 3.0 \<USO EXTERNO RESTRINGIDO\> 18/04/2022
>
> 24

<img src="./cvfstquw.png" style="width:4.09861in" />

> <img src="./mfpymprz.png"
> style="width:1.05208in;height:0.40625in" />***Integración***
> ***inSite***
>
> **8.2** **EMV3DS**
>
> **8.2.1** **Iniciar** **Petición**
>
> Esta petición permite obtener el tipo de autenticación 3D Secure que
> se puede realizar, además de la URL del 3DSMethod, en caso de que
> exista.
>
> Eliniciapeticiónsehacea través deuna petición REST al TPV Virtual
> Endichapetición deberá incluir los siguientes parámetros:
>
> • Ds_SignatureVersion: Constante que indica la versión de firma que se
> está utilizando.
>
> • Ds_MerchantParameters: Cadena en formato JSON con todos los
> parámetros de la petición codificada en Base 64 y sin retornos de
> carro (Consultar [Parámetros de entrada y
> salida)](file://///smp2020.redsys.local/AEA/NUEVOS%20CANALES/TPVVIRTUAL/DOCUMENTACION_NUEVA/GuÃ­a%20IntegraciÃ³n/Manual%20IntegraciÃ³n%20-%20Rest/TPV-Virtual%20Manual%20IntegraciÃ³n%20-%20Rest%20V2.4%20-%20(en%20vigor%20-%20NO%20MODIFICAR%20-%20enviada%20a%20entidades%2012-11-2019)%20.docx%23_ParÃ¡metros_de_entrada).
>
> • Ds_Signature: Firma de los datos enviados. Es el resultado del HMAC
> SHA256 de la cadena JSON codificada en Base 64 enviada en el parámetro
> anterior
>
> Dichos parámetros deben enviarse a los siguientes endpoints
> dependiendo de si se quiere realizar una petición en el entorno de
> prueba u operaciones reales:
>
> <img src="./trbzq5si.png"
> style="width:4.61458in;height:4.61458in" /><img src="./imszcy50.png" style="width:4.09861in" />Una
> vez gestionada la petición, el TPV Virtual informará al servidor del
> comercio el resultado de esta, con la información del resultado
> incluida en un fichero JSON. En él se incluirán los siguientes campos:
>
> • Ds_SignatureVersion: Constante que indica la versión de firma que se
> está utilizando.
>
> • Ds_MerchantParameters: Cadena en formato JSON con todos los
> parámetros de la respuesta codificada en Base 64 y sin retornos de
> carro.
>
> • Ds_Signature: Firma de los datos enviados. Resultado del HMAC SHA256
> de la cadena JSON codificada en Base 64 enviada en el parámetro
> anterior. **El** **comercio** **es** **responsable** **de**
> **validar** **el** **HMAC** **enviado** **por** **el** **TPV**
> **Virtual** **para** **asegurarse** **de** **la** **validez** **de**
> **la** **respuesta.** **Esta** **validación** **es** **necesaria**
> **para** **garantizar** **que** **los** **datos** **no** **han**
> **sido** **manipulados** **y** **que** **el** **origen** **es**
> **realmente** **el** **TPV** **Virtual.**
>
> Redsys · C/ Francisco Sancha, 12 · 28034 · Madrid · ESPAÑA
>
> Versión: 3.0 \<USO EXTERNO RESTRINGIDO\> 18/04/2022
>
> 25

<img src="./hwazsrpj.png" style="width:4.09861in" />

> <img src="./blbywus3.png"
> style="width:1.05208in;height:0.40625in" />***Integración***
> ***inSite***
>
> A continuación, se describen los datos que debe incluir el
> Ds_MerchantParameters para enviar un inicia petición al Servicio REST:
>
> {
>
> "DS_MERCHANT_ORDER":"1552571678",
> "DS_MERCHANT_MERCHANTCODE":"999008881", "DS_MERCHANT_TERMINAL":"2",
> "DS_MERCHANT_CURRENCY":"978", "DS_MERCHANT_TRANSACTIONTYPE":"0",
> "DS_MERCHANT_AMOUNT":"1000",
>
> **"DS_MERCHANT_IDOPER":"**
> **455097a74c21b761be86acb26c32609dce222e66",**
> **"DS_MERCHANT_EMV3DS":** **{"threeDSInfo":"CardData"**}
>
> }
>
> Como respuesta se obtendrá lo siguiente:
>
> {
>
> "Ds_Order":"1552571678", "Ds_MerchantCode":"999008881",
> "Ds_Terminal":"2", "Ds_TransactionType":"0", **"Ds_EMV3DS":** **{**
>
> **"protocolVersion":"2.1.0",**
>
> **"threeDSServerTransID":"8de84430-3336-4ff4-b18d-f073b546ccea",**
> **"threeDSInfo":"CardConfiguration",**
>
> **"threeDSMethodURL[":<u>https://sis.redsys.es/sis-simulador-web/threeDsMethod.jsp</u>](https://sis.redsys.es/sis-simulador-web/threeDsMethod.jsp)**
>
> **}** }
>
> El parámetro **Ds_EMV3DS** estará compuesto por los siguientes campos:
>
> • protocolVersion: siempre indicará el número de versión mayor
> permitido en la operación. El comercio será responsable de utilizar el
> número de versión para el cual esté preparado.
>
> <img src="./oypbgjd1.png"
> style="width:4.61458in;height:4.61458in" /><img src="./sjqkgabv.png" style="width:4.09861in" />•
> threeDSServerTransID: identificador de la transacción EMV3DS. •
> threeDSInfo: CardConfiguration.
>
> • threeDSMethodURL: URL del 3DSMethod.
>
> **8.2.2** **Ejecución** **del** **3DSMethod**
>
> El3DSMethodes unprocesoquepermitea la entidademisora capturarla
> información del dispositivo que está utilizando el titular. Esta
> información, junto con los datos EMV3DS,que son enviados enla
> autorización, será utilizada porla entidadpara hacer una evaluación
> del riesgo de la transacción. En base a esto, el emisor puede
> determinar que la transacción es confiable y por lo tanto no requerir
> la intervención del titular para verificar su autenticidad
> (frictionless).
>
> La captura de datos del dispositivo se realiza mediante un iframe
> oculto en el navegador del cliente, que establecerá conexión
> directamente con la entidad emisora de forma transparente para el
> usuario. El comercio recibirá una notificación cuanto haya
> terminadolacaptura deinformacióny enelsiguientepaso, al
> realizarlapetición de autorización al TPV Virtual el comercio deberá
> enviar el parámetro threeDSCompInd indicando la ejecución del
> 3DSMethod.
>
> Pasos para la ejecución del 3DSMethod:
>
> Redsys · C/ Francisco Sancha, 12 · 28034 · Madrid · ESPAÑA
>
> Versión: 3.0 \<USO EXTERNO RESTRINGIDO\> 18/04/2022
>
> 26

<img src="./xa3bnkyf.png" style="width:4.09861in" />

> <img src="./kw240ak5.png"
> style="width:1.05208in;height:0.40625in" />***Integración***
> ***inSite***
>
> 1\. En la respuesta recibida con la configuración de la tarjeta
> (iniciaPeticion) se recibe los datos siguientes para ejecutar el
> 3DSMethod:
>
> a\. threeDSMethodURL: url del 3DSMethod
>
> b\. threeDSServerTransID: Identificador de transacción EMV3DS
>
> Si en la respuesta no se recibe threeDSMethodURL el proceso finaliza.
> En la autorización enviar threeDSCompInd = N
>
> 2\. Construir el JSON Object con los parámetros:
>
> a\. <u>threeDSServerTransID</u>: valor recibido en la respuesta de
> consulta de tarjeta
>
> b\. <u>threeDSMethodNotificationURL</u>: url del comercio a la que
> será notificada la finalización del 3DSMethod desde la entidad
>
> 3\. Codificar el JSON anterior en Base64url encode
>
> 4\. Debe incluirse un iframe oculto en el navegador del cliente, y
> enviar un campo **threeDSMethodData** con el valor del objecto json
> anterior, en un formulario http post a la url obtenida en la consulta
> inicial **threeDSMethodURL**
>
> 5\. La entidad emisora interactúa con el browser para proceder a la
> captura de información. Al finalizar enviará el campo
> **threeDSMethodData** en el iframe html del navegador por http post a
> la url **threeDSMethodNotificationURL** (indicada en el paso 2), y el
> 3DSMethod termina.
>
> <img src="./kdsquhba.png"
> style="width:4.61458in;height:4.61458in" /><img src="./0kwnjk5n.png" style="width:4.09861in" />6.
> Si el 3DSMethod se ha completado en menos de 10 segundos se enviará
> **threeDSCompInd** **=** **Y** en la autorización. Si no se ha
> completado en 10 segundos debe detener la espera y enviar la
> autorización con **threeDSCompInd** **=** **N**
>
> Redsys · C/ Francisco Sancha, 12 · 28034 · Madrid · ESPAÑA
>
> Versión: 3.0 \<USO EXTERNO RESTRINGIDO\> 18/04/2022
>
> 27

<img src="./y2cl2ekk.png" style="width:4.09861in" />

> <img src="./ogawc4gc.png"
> style="width:1.05208in;height:0.40625in" />***Integración***
> ***inSite***
>
> **8.2.3** **Petición** **de** **autorización** **con** **datos**
> **EMV3DS**
>
> La petición de autorización se hace a través de una petición REST al
> TPV Virtual En dicha petición deberá incluir los siguientes
> parámetros:
>
> • Ds_SignatureVersion: Constante que indica la versión de firma que se
> está utilizando.
>
> • Ds_MerchantParameters: Cadena en formato JSON con todos los
> parámetros de la petición codificada en Base 64 y sin retornos de
> carro (Consultar [Parámetros de entrada y
> salida)](file://///smp2020.redsys.local/AEA/NUEVOS%20CANALES/TPVVIRTUAL/DOCUMENTACION_NUEVA/GuÃ­a%20IntegraciÃ³n/Manual%20IntegraciÃ³n%20-%20Rest/TPV-Virtual%20Manual%20IntegraciÃ³n%20-%20Rest%20V2.4%20-%20(en%20vigor%20-%20NO%20MODIFICAR%20-%20enviada%20a%20entidades%2012-11-2019)%20.docx%23_ParÃ¡metros_de_entrada).
>
> • Ds_Signature: Firma de los datos enviados. Es el resultado del HMAC
> SHA256 de la cadena JSON codificada en Base 64 enviada en el parámetro
> anterior
>
> Dichos parámetros deben enviarse a los siguientes endpoints
> dependiendo de si se quiere realizar una petición en el entorno de
> prueba u operaciones reales:

||
||
||
||
||

> Una vez gestionada la consulta, el TPV Virtual informará al servidor
> del comercio el resultado de la misma con la información del resultado
> incluida en un fichero JSON. En él se incluirán los siguientes campos:
>
> <img src="./wmbc2szh.png"
> style="width:4.61458in;height:4.61458in" /><img src="./jdq0vi2l.png" style="width:4.09861in" />•
> Ds_SignatureVersion: Constante que indica la versión de firma que se
> está utilizando.
>
> • Ds_MerchantParameters: Cadena en formato JSON con todos los
> parámetros de la respuesta codificada en Base 64 y sin retornos de
> carro.
>
> • Ds_Signature: Firma de los datos enviados. Resultado del HMAC SHA256
> de la cadena JSON codificada en Base 64 enviada en el parámetro
> anterior. **El** **comercio** **es** **responsable** **de**
> **validar** **el** **HMAC** **enviado** **por** **el** **TPV**
> **Virtual** **para** **asegurarse** **de** **la** **validez** **de**
> **la** **respuesta.** **Esta** **validación** **es** **necesaria**
> **para** **garantizar** **que** **los** **datos** **no** **han**
> **sido** **manipulados** **y** **que** **el** **origen** **es**
> **realmente** **el** **TPV** **Virtual.**
>
> A continuación, se describen los datos de debe incluir el
> Ds_MerchantParameters para enviar una petición de autorización con
> autenticación EMV3DS al Servicio REST:
>
> Redsys · C/ Francisco Sancha, 12 · 28034 · Madrid · ESPAÑA
>
> Versión: 3.0 \<USO EXTERNO RESTRINGIDO\> 18/04/2022
>
> 28

<img src="./j2wdpilq.png" style="width:4.09861in" />

> <img src="./uvndit2x.png"
> style="width:1.05208in;height:0.40625in" />***Integración***
> ***inSite***
>
> {
>
> "DS_MERCHANT_ORDER":1552572812,
> "DS_MERCHANT_MERCHANTCODE":"999008881", "DS_MERCHANT_TERMINAL":"2",
> "DS_MERCHANT_CURRENCY":"978", "DS_MERCHANT_TRANSACTIONTYPE":"0",
> "DS_MERCHANT_AMOUNT":"1000",
>
> **"DS_MERCHANT_IDOPER":"**
> **455097a74c21b761be86acb26c32609dce222e66",**
> **"DS_MERCHANT_EMV3DS":**
>
> **{** **"threeDSInfo":"AuthenticationData",**
>
> **"protocolVersion":"2.1.0",**
> **"browserAcceptHeader":"text/html,application/xhtml+xml,application/xml;q=0.9,\*/\*;q=0.8,application/json",**
>
> **"browserUserAgent":"Mozilla/5.0** **(WindowsNT** **10.0;**
> **Win64;** **x64)** **AppleWebKit/537.36** **(KHTML,** **like**
> **Gecko)** **Chrome/71.0.3578.98** **Safari/537.36",**
>
> **"browserJavaEnabled":"false",**
> **"browserJavaScriptEnabled":"false",**
>
> **"browserLanguage":"ES-es",** **"browserColorDepth":"24",**
> **"browserScreenHeight":"1250",** **"browserScreenWidth":"1320",**
> **"browserTZ":"52",**
>
> **"threeDSServerTransID":"8de84430-3336-4ff4-b18d-f073b546ccea",**
> **"notificationURL":"https://comercio-inventado.es/recibe-respuesta-autenticacion",**
> **"threeDSCompInd":"Y"**
>
> **}** }
>
> Como respuesta se obtendrá:
>
> • Si se hace un Frictionless, se obtendrá directamente el resultado
> final de la operación:
>
> {
>
> <img src="./1yxpiill.png"
> style="width:4.61458in;height:4.61458in" /><img src="./wbkcwdtw.png" style="width:4.09861in" />"Ds_Amount":"1000",
> "Ds_Currency":"978", "Ds_Order":"1552572812",
> "Ds_MerchantCode":"999008881", "Ds_Terminal":"2",
> "Ds_Response":"0000", "Ds_AuthorisationCode":"694432",
> "Ds_TransactionType":"0", "Ds_SecurePayment":"1", "Ds_Language":"1",
> "Ds_CardNumber":"454881\*\*\*\*\*\*0004", "Ds_Card_Type":"C",
> "Ds_MerchantData":"", "Ds_Card_Country":"724", "Ds_Card_Brand":"1"
>
> }
>
> • Si no es así, se solicitará la ejecución de un Challenge:
>
> {
>
> "Ds_Amount":"1000", "Ds_Currency":"978", "Ds_Order":"1552572812",
> "Ds_MerchantCode":"999008881", "Ds_Terminal":"2",
> "Ds_TransactionType":"0", **"Ds_EMV3DS":{**
>
> **"threeDSInfo":"ChallengeRequest",** **"protocolVersion":"2.1.0",**
>
> **"acsURL":"https://sis.redsys.es/sis-simulador-web/authenticationRequest.jsp",**
>
> Redsys · C/ Francisco Sancha, 12 · 28034 · Madrid · ESPAÑA
>
> Versión: 3.0 \<USO EXTERNO RESTRINGIDO\> 18/04/2022
>
> 29

<img src="./vuqhpffl.png" style="width:4.09861in" />

> <img src="./lixkvlqv.png"
> style="width:1.05208in;height:0.40625in" />***Integración***
> ***inSite***
>
> **"creq":"eyJ0aHJlZURTU2VydmVyVHJhbnNJRCI6IjhkZTg0NDMwLTMzMzYtNGZmNC1iMThkLWYwNzNi**
> **NTQ2Y2NlYSIsImFjc1RyYW5zSUQiOiJkYjVjOTljNC1hMmZkLTQ3ZWUtOTI2Zi1mYTBiMDk0MzUyYTAiLC**
> **JtZXNzYWdlVHlwZSI6IkNSZXEiLCJtZXNzYWdlVmVyc2lvbiI6IjIuMS4wIiwiY2hhbGxlbmdlV2luZG93U2l6ZSI**
> **6IjA1In0"**
>
> **}** }
>
> **8.2.4** **Ejecución** **del** **Challenge**
>
> Describimos este proceso en 3 pasos:
>
> <u>Paso 1</u>.- Conexión desde el comercio el ACS del banco emisor
>
> El siguiente paso consiste en conectar desde el comercio con la
> entidad emisora para que el cliente se pueda autenticar. Esta conexión
> se hace enviando un formulario http POST a la url del ACS del banco.
> Para esta conexión utilizamos los datos recibidos en el parámetro
> Ds_EMV3DS del paso anterior (parámetros acsURL y creq):
>
> “Ds_EMV3DS”: {"threeDSInfo":"ChallengeRequest",
> "protocolVersion":"2.1.0",
>
> **"acsURL":"https://sis.redsys.es/sis-simulador-web/authenticationRequest.jsp",**
> **"creq":"eyJ0aHJlZURTU2VydmVyVHJhbnNJRCI6ImU5OWMzYzI2LTFiZWItNGY4NS05ZmE3LTI3OTJiZjE5**
> **NDZlMiIsImFjc1RyYW5zSUQiOiIyMTQzNDFhYi0wMjlhLTRmMGEtOTEyNi1iMDFkYmE5OTc2MTkiLCJtZX**
> **NzYWdlVHlwZSI6IkNSZXEiLCJtZXNzYWdlVmVyc2lvbiI6IjIuMS4wIiwiY2hhbGxlbmdlV2luZG93U2l6ZSI6IjA**
> **1In0"**}
>
> Ejemplo:
>
> <img src="./4iz2szmw.png"
> style="width:4.61458in;height:4.61458in" /><img src="./1wbr1pmt.png" style="width:4.09861in" />\<form
> action="{acsURL}" method="POST" enctype =
> "application/x-www-form-urlencoded"\> \<input type="hidden"
> name="creq" value="{creq}” "\>
>
> \</form\>
>
> Con los datos recibidos en Ds_EMV3DS sería:
>
> \<form
> action="https://sis.redsys.es/sis-simulador-web/authenticationRequest.jsp"
> method="POST" enctype = "application/x-www-form-urlencoded"\>
>
> \<input type="hidden" name="creq"
> value="eyJ0aHJlZURTU2VydmVyVHJhbnNJRCI6ImU5OWMzYzI2LTFiZWItNGY4NS05ZmE3LTI3OTJiZjE5NDZlMiIsImFjc1Ry
> YW5zSUQiOiIyMTQzNDFhYi0wMjlhLTRmMGEtOTEyNi1iMDFkYmE5OTc2MTkiLCJtZXNzYWdlVHlwZSI6IkNSZXEiLCJtZXNzY
> WdlVmVyc2lvbiI6IjIuMS4wIiwiY2hhbGxlbmdlV2luZG93U2l6ZSI6IjA1In0"\>
>
> \</form\>
>
> <u>Paso 2</u>.- Ejecución del challenge
>
> El titular se autentica por los métodos que le exija su entidad
> emisora: OTP, contraseña estática, biometría, etc.
>
> <u>Paso 3</u>.- Recepción del resultado de la autenticación
>
> Una vez finalizado el challenge la entidad emisora enviará el
> resultado al comercio, haciendo un http POST a la url del parámetro
> *notificationURL* que el comercio envió previamente en la petición de
> autorización:
>
> **"notificationURL":"**
> **https://comercio-inventado.es/recibe-respuesta-autenticacion"**
>
> Redsys · C/ Francisco Sancha, 12 · 28034 · Madrid · ESPAÑA
>
> Versión: 3.0 \<USO EXTERNO RESTRINGIDO\> 18/04/2022
>
> 30

<img src="./2r4a1ig5.png" style="width:4.09861in" />

> <img src="./fxla4jdf.png"
> style="width:1.05208in;height:0.40625in" />***Integración***
> ***inSite***
>
> El comercio recibirá el parámetro “cres” que utlizará en la petición
> de autorización final que vemos en el siguiente apartado.
>
> **8.2.5** **Confirmación** **de** **autorización** **EMV3DS**
> **posterior** **al** **Challenge**
>
> A continuación, se describen los datos de debe incluir el
> Ds_MerchantParameters para enviar una petición de confirmación de
> autorización EMV3DS al Servicio REST:
>
> {
>
> "DS_MERCHANT_ORDER":1552577128,
> "DS_MERCHANT_MERCHANTCODE":"999008881", "DS_MERCHANT_TERMINAL":"2",
> "DS_MERCHANT_CURRENCY":"978", "DS_MERCHANT_TRANSACTIONTYPE":"0",
> "DS_MERCHANT_AMOUNT":"1000", "DS_MERCHANT_PAN":" XXXXXXXXXXXXXXXXXX ",
> "DS_MERCHANT_EXPIRYDATE":"XXXX", "DS_MERCHANT_CVV2":"XXX",
> **"DS_MERCHANT_EMV3DS":{**
>
> **"threeDSInfo":"ChallengeResponse",** **"protocolVersion":"2.1.0",**
>
> **"cres":"eyJ0aHJlZURTU2VydmVyVHJhbnNJRCI6IjhkZTg0NDMwLTMzMzYtNGZmNC1iMThkLW**
> **YwNzNiNTQ2Y2NlYSIsImFjc1RyYW5zSUQiOiJkYjVjOTljNC1hMmZkLTQ3ZWUtOTI2Zi1mYTBiM**
> **Dk0MzUyYTAiLCJtZXNzYWdlVHlwZSI6IkNSZXMiLCJtZXNzYWdlVmVyc2lvbiI6IjIuMS4wIiwidHJ**
> **hbnNTdGF0dXMiOiJZIn0="**
>
> **}** }
>
> Como respuesta se obtendrá el resultado final de la operación:
>
> <img src="./5azsskic.png"
> style="width:4.61458in;height:4.61458in" /><img src="./y0skt5l0.png" style="width:4.09861in" />{
>
> "Ds_Amount":"1000", "Ds_Currency":"978", "Ds_Order":"1552572812",
> "Ds_MerchantCode":"999008881", "Ds_Terminal":"2",
> "Ds_Response":"0000", "Ds_AuthorisationCode":"694432",
> "Ds_TransactionType":"0", "Ds_SecurePayment":"1", "Ds_Language":"1",
> "Ds_CardNumber":"454881\*\*\*\*\*\*0004", "Ds_Card_Type":"C",
> "Ds_MerchantData":"", "Ds_Card_Country":"724", "Ds_Card_Brand":"1"
>
> }
>
> **NOTA:** **Para** **mayor** **detalle** **sobre** **el**
> **protocolo** **EMV3DS** **2.0,** **se** **deben** **consultar**
> **la** **guía** **de** **Integración** **vía** **REST** **del**
> **TPV-Virtual.**
>
> Redsys · C/ Francisco Sancha, 12 · 28034 · Madrid · ESPAÑA
>
> Versión: 3.0 \<USO EXTERNO RESTRINGIDO\> 18/04/2022
>
> 31
