 
var listaItems = [];
var tipos = document.getElementsByClassName("sub-menu-tipo");
cuerpoLista = document.getElementById("sortable");

for (var i = 0; i < tipos.length; i++) {
    tipos[i].onclick = function () {
                
        if (listaItems.length === 0) {
            seleccionarTipo(this);
            agregarFila(this.innerHTML);
        }
        else if (listaItems.length > 0) {
            if (!isInArray(this.innerHTML, listaItems)) {
                 seleccionarTipo(this);
                 agregarFila(this.innerHTML);
            } else {
                eliminarItem(this.innerHTML.replace(/ /g,"_"));
                deseleccionarTipo(this.innerHTML);
            }
        } 
    }
}
function eliminarItem(id) {
    
    $("#sortable").children("#"+id).remove();
}

function isInArray(value, array) {
    return array.indexOf(value) > -1;
}

function agregarFila(html) {
    filaLista = crearLi(html);
    filaLista.appendChild(crearBoton());
    cuerpoLista.appendChild(filaLista);
}

function crearDivNumero() {
    div = document.createElement('div');
    div.className = "numero";
    return div;
}

function crearDiv(id) {
    div = document.createElement('div');
    div.className = "lista-item";
    div.innerHTML = id;
    return div;
}

function crearLi(id) {
    li = document.createElement('li');
    li.id = id.replace(/ /g,"_");
    li.appendChild(crearDivNumero());
    li.appendChild(crearDiv(id));
    li.className = "ui-state-default";
    return li;
}

function crearBoton() {
    btn = document.createElement('input');
    btn.type= 'button';
    btn.value = 'x';
    btn.className = 'close';
    btn.onclick = function(){
        this.parentNode.parentNode.removeChild(this.parentNode);
        item = this.parentNode.getElementsByClassName('lista-item')[0].innerHTML;
        deseleccionarTipo(item);
    }
    return btn;
}

function seleccionarTipo(tipo) {
    tipo.style.backgroundColor = "#03A9F4";
    tipo.style.color = "#ffffff";
    
}

function deseleccionarTipo(item) {
    item = item.replace(/ /g, "_");
    document.getElementById("tipo-" + item).style.backgroundColor = "#fff";
    document.getElementById("tipo-" + item).style.color = "#222222";
}

$("#Almacen").click(function (event) {
    $("#sub-menu-almacen").show();
});
        
$(function () {
    $("#tabs").tabs({
        event: "mouseover"
    });

    $("#tabs").tabs().addClass("ui-tabs-vertical ui-helper-clearfix");
    $("#tabs li").removeClass("ui-corner-top").addClass("ui-corner-left");
});

$(function () {
    $("#sortable").sortable();
    $("#sortable").disableSelection();
});

function organizarNumeros() {
    $(".numero").each(function () {
        $(this).html($(this).parent().index()+1 + "- ");
    });
}

function obtenerListaItems() {
    listaItems= [];
    $(".lista-item").each(function () {
        listaItems.push($(this).html());
    });
}

html = $('#cuerpo-lista').html();
setInterval(function () {
    if ($('#cuerpo-lista').html() != html) {
        organizarNumeros();
        obtenerListaItems();
        html = $('#cuerpo-lista').html();
        if (listaItems.length > 0) {
            $("#opciones-lista").show();
            $("#guardar-lista").show();
            $("#mensaje-lista").html("");
        } else {
            $("#opciones-lista").hide();
            $("#guardar-lista").hide();
            $("#mensaje-lista").html("Lista vac&iacute;a inserte nuevos elementos");
        }
    }
    
   
}, 10);

$(function () {
    $("#draggable").draggable();
});

$("#minimizar").click(function () {
    $("#estacion").show();
    $("#panel-lista").hide();
});

$("#boton-estacion").click(function () {
    $("#panel-lista").show();
    $("#estacion").hide();
});

function klk() { alert("klk"); }

$(function () {
    $("#dialogo-guardar-lista").dialog({
        autoOpen: false,
        resizable: false,
        width: "600px"
    });
    $("#dialogo-guardar-lista").dialog({
        autoOpen: false
    });
});

$("#guardar-lista").click(function () {
    $("#dialogo-guardar-lista").dialog("open");
    editarBotonCerrarDialogos();
});
function postLista() {
    var postData = { values: listaItems };
    
    $.ajax({
        type: "POST",
        url: "/lista/CreateWithJson",
        data: postData,
        success: function (data) {
            alert(data.Result);
        },
        dataType: "json",
        traditional: true
    });
}

function editarBotonCerrarDialogos() {
    document.getElementsByClassName("ui-icon-closethick")[0].style.left = "0";
    document.getElementsByClassName("ui-icon-closethick")[0].style.top = "0";
    document.getElementsByClassName("ui-icon-closethick")[1].style.left = "0";
    document.getElementsByClassName("ui-icon-closethick")[1].style.top = "0";
}

$("#dialogo-utilizar-continuar").click(function () {
    if ($('#dialogo-utilizar-nombre').val() == "") {
        if ($('#dialogo-utilizar-nombre').val() == "") {
            $('#mensaje-escribe-nombre').html("Escribe el nombre de la lista.");
        } else {
            $('#mensaje-escribe-nombre').html("");
        }
    } else {
        utilizarLista();
        return true;
    }
    console.log("nombre lista: " + $('#panel-utilizar-nombre').val());
});

function utilizarLista() {
    var utilizarLista = [];
    utilizarLista.push(document.getElementById("dialogo-utilizar-nombre").value);
    
    for (var i = 0; i < listaItems.length ; i++) {
        utilizarLista.push(listaItems[i]);
    }
    var postData = { values: utilizarLista };

    $.ajax({
        type: "POST",
        url: "/lista/utilizarLista",
        data: postData,
        success: function (data) {
            
            if (data === true) {
                console.log("ajax success: true");
                window.location = "lista/miLista";

            } else {
                console.log("ajax success: false");
                $('#mensaje-lista-existe').html("Ya tienes una lista con este nombre");
            }
        },
        error: function (xhr, status, error) {
            console.log("ajax error");
        },
        dataType: "json",
        traditional: true
    });
}

//------------------------------------FIN  dialogo-utilizar-lista----


