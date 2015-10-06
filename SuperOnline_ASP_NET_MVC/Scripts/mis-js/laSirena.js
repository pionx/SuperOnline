//-------------------------------------carrito
$('#imagen-super').html("<a href='producto'><img id='super-imagen' width='230px' height='165px' class='col-lg-12' src='/Images/mis-imagenes/super-mercados/La sirena.png'></a>");

var carrito = [];

obtenerCarritoLaSirenaDeSession();

$('article').click(function () {

    var articulo = { nombre: "", cant: 0, precio: 0, total: 0 };
    //remover (articulo-) de el id
    nombreSinEspacio = $(this).attr("id").replace('articulo-', '');
    //remplazar (_) por espacios en blanco
    nombre = nombreSinEspacio.replace(/_/g, " ");


    for (var i = 0; i < carrito.length; i++) {

        if (carrito[i].nombre === nombre) {
            alert("Este producto ya está en carrito");
            return false;
        }
    }

    precioConRD = $(this).find('.articulo-precio').html();
    precioSinRD = precioConRD.replace('RD$', '');
    precio = parseFloat(precioSinRD);
    total = parseFloat(precio);
    articulo.nombre = nombre;
    articulo.precio = precio;
    articulo.cant = 1;
    articulo.total = total;
    carrito.push(articulo);
    agregarArticuloAlDivCarrito(articulo);
});

function obtenerCarritoLaSirenaDeSession() {
    var postData = { values: "" };

    $.ajax({
        type: "GET",
        url: "/Carrito/obtenerCarritoLaSirenaDeSession",
        success: function (data) {
            if (data == false) {
                console.log("ajax success: false, al cargar carrito de session");
            } else {
                for (var i = 0; i < data.length; i++) {
                    //carrito[i] = data[i];
                    //carrito = _(data).toArray();
                    carrito = JSON.parse(data);
                    agregarTodosArticulosAlDivCarrito();
                }
                console.log("Carrito cargado de session");
            }
        },
        error: function (xhr, status, error) {
            console.log("ajax error: al cargar carrito de session");
        },
        dataType: "json",
        traditional: true
    });
}

function guardarCarritoLaSirenaEnSession() {
    var postData = { articulos: JSON.stringify(carrito) };

    $.ajax({
        type: "POST",
        url: "/Carrito/guardarCarritoLaSirenaEnSession",
        data: postData,
        success: function (data) {
            if (data == false) {
                console.log("ajax success: false, carrito no guardado");
            } else {
                console.log("ajax success: true, carrito guardado en session");
            }
        },
        error: function (xhr, status, error) {
            console.log("ajax error: when getting carrito");
        },
        dataType: "json",
        traditional: true
    });
}

function agregarTodosArticulosAlDivCarrito() {
    if (carrito.length > 0) {
        for (var i = 0; i < carrito.length; i++) {
            console.log(i + "- agregar articulo a div");
            agregarArticuloAlDivCarrito(carrito[i]);
        }
    }
}

function agregarArticuloAlDivCarrito(articulo) {
    articuloDiv = crearDivarticulo();
    imgDiv = crearDivImg("/Images/mis-imagenes/Imagenes-productos/" + articulo.nombre + ".PNG"); 
    detallesDiv = crearDivDetalles();
    nombreDiv = crearDivNombre(articulo.nombre);
    cantDiv = crearDivCant(articulo.cant);
    precioDiv = crearDivPrecio(articulo.precio);
    totalDiv = crearDivTotal(articulo.total);
    opcionesDiv = crearDivOpciones(articulo.nombre);

    detallesDiv.appendChild(nombreDiv);
    detallesDiv.appendChild(cantDiv);
    detallesDiv.appendChild(precioDiv);
    detallesDiv.appendChild(totalDiv);
    detallesDiv.appendChild(opcionesDiv);

    articuloDiv.appendChild(imgDiv);
    articuloDiv.appendChild(detallesDiv);
    $(articuloDiv).hide().appendTo("#carrito-contenido").fadeIn(1000);

    $(".select-cant").change(function () {

        nombre = $(this).parent().parent().children('.carrito-articulo-nombre').html();

        for (var i = 0; i < carrito.length; i++) {

            if (carrito[i].nombre === nombre) {
                carrito[i].cant = $(this).val();
                carrito[i].total = parseFloat($(this).val()) * carrito[i].precio;
                guardarCarritoLaSirenaEnSession();
                $(this).parent().parent().children('.carrito-articulo-total').html("Total: RD$" + carrito[i].total);
            }
        }
    });

    $('.eliminar-articulo').click(function () {
        $(this).parent().parent().parent().fadeOut(400);
        nombre = $(this).parent().parent().children('.carrito-articulo-nombre').html();

        for (var i = 0; i < carrito.length; i++) {
            if (carrito[i].nombre === nombre) {
                carrito.splice(i, 1);
            }
        }
        guardarCarritoLaSirenaEnSession();
    });
    guardarCarritoLaSirenaEnSession();

}
function crearDivarticulo() {
    div = document.createElement('div');
    div.className = 'carrito-articulo col-lg-12';
    return div;
}
function crearDivImg(src) {
    div = document.createElement('div');
    div.className = 'carrito-articulo-img col-lg-2';
    img = document.createElement('img');
    img.src = src;
    img.width = '70';
    img.height = '70';
    div.appendChild(img);
    return div;
}
function crearDivDetalles() {
    div = document.createElement('div');
    div.className = 'carrito-articulo-detalles col-lg-10';
    return div;
}
function crearDivNombre(nombre) {
    div = document.createElement('div');
    div.className = 'carrito-articulo-nombre col-lg-12';
    div.innerHTML = nombre;
    return div;
}
function crearDivCant(cant) {
    div = document.createElement('div');
    div.className = 'carrito-articulo-cant col-lg-2';
    div.appendChild(crearSelect(50, cant));
    return div;
}
function crearSelect(n, cant) {
    select = document.createElement("select");
    select.className = 'form-control select-cant';
    for (var i = 1; i < n; i++) {
        option = document.createElement("option");
        option.text = i;
        option.value = i;
        select.appendChild(option);
    }

    select.selectedIndex = cant - 1;
    return select;
}
function crearDivPrecio(precio) {
    div = document.createElement('div');
    div.className = 'carrito-articulo-precio col-lg-4';
    div.innerHTML = "RD$" + precio;
    return div;
}
function crearDivTotal(total) {
    div = document.createElement('div');
    div.className = 'carrito-articulo-total col-lg-6';
    div.innerHTML = "Total: RD$" + total;
    return div;
}
function crearDivOpciones(nombre) {
    div = document.createElement('div');
    div.innerHTML = "<a class='eliminar-articulo'>Eliminar</a>";
    div.className = 'col-lg-12';
    return div;
}

document.getElementById('hacer-pedido').disabled = "disabled";
document.getElementById('revisar-carrito').disabled = "disabled";

setInterval(function () {
    carritoTotal = 0;
    if (carrito != null) {
        for (var i = 0; i < carrito.length; i++) {
            carritoTotal += carrito[i].total;

        }
        $('#titulo-carrito').html("Carrito | Total general: RD$" + carritoTotal);

        if (carrito.length > 0) {
            $('#hacer-pedido').prop("disabled", false);
            $('#revisar-carrito').prop("disabled", false);
            $('#mendaje-carrito-vacio').fadeOut();

        } else {

            $('#mendaje-carrito-vacio').fadeIn();
            $('#hacer-pedido').attr("disabled", "disabled");
            $('#revisar-carrito').attr("disabled", "disabled");
        }
    }
}, 10);


//--------------------------Lista

$('#guardar-lista').click(function () {
    var postData = { values: listaCompras };

    $.ajax({
        type: "POST",
        url: "/lista/GuardarLista",
        //url: "/lista/CreateWithJson",
        data: postData,
        success: function (data) {

            if (data === true) {
                console.log("ajax success: true");


            } else {
                console.log("ajax success: false");
                $("#dialogo-guardar-lista").dialog("open");
            }
        },
        error: function (xhr, status, error) {
            console.log("ajax error");
        },
        dataType: "json",
        traditional: true
    });
});


$(function () {
    $("#dialogo-guardar-lista").dialog({
        autoOpen: false,
        resizable: false,
        width: "350px"
    });

});

contador = 0;
function listaItem(item) {
    contador++;
    itemSinEspacio = item.replace(/ /g, "_");
    div = document.createElement('div');
    div.innerHTML = contador + "- " + item;
    div.id = "lista-compras-" + itemSinEspacio;
    div.onclick = function () {
        window.location.href = "?tipo=" + item;
        $('mostrar-tipo').html(item);
    }
    div.className = 'lista-hover';
    return div;

}

//-------------------------------------FIN LISTA

$('#super-detalles').hide();

$('#otro-super').click(function () {
    $('#super-detalles').fadeIn();
});

$('#cerrar').click(function () {
    $('#super-detalles').fadeOut();
});

$('#buscar').click(function () {
    if ($('#txtBuscar').val() != "") {
        txtBuscar = $('#txtBuscar').val();
        console.log("buscar: " + txtBuscar);
        window.location.href = buscarUrl + "?buscar=" + txtBuscar;
    }
});
