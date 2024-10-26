/*Mostar los controles */
const VISTA_BUSQUEDA = {
    busquedaFecha: () => {
        /*Función anónima de como mostrar las cajas de texto
        Para mostrar las cajas de texto de fecha o ocultarla*/

        //Se limpian las cajas de texto
        $("#txtFechaInicio").val("")
        $("#txtFechaFin").val("")
        $("#txtNumeroVenta").val("")

        /*Llamado a la búsqueda fecha */
        $(".busqueda-fecha").show()
        $(".busqueda-venta").hide()
    }, busquedaVenta: () => {
        /*Función anónima de como mostrar las cajas de texto
        Para mostrar las cajas de texto de fecha o ocultarla*/

        //Se limpian las cajas de texto
        $("#txtFechaInicio").val("")
        $("#txtFechaFin").val("")
        $("#txtNumeroVenta").val("")

        /*Llamado a la búsqueda fecha */
        $(".busqueda-fecha").hide()
        $(".busqueda-venta").show()
    }
}

//Para ver cuando nuestro ducumento ya esté cargado

$(document).ready(function () {
    VISTA_BUSQUEDA["busquedaFecha"]()//Los parentesis son para ejecutar la propiedad busqueda fecha de arriba

    //Se va configurar el idioma del calendario
    $.datepicker.setDefaults($.datepicker.regional["es"])

    //Se configura la caja de texto para que se muestre el calendario
    $("#txtFechaInicio").datepicker({ dateFormat : "dd/mm/yy"})
    $("#txtFechaFin").datepicker({ dateFormat: "dd/mm/yy"})
})

//Cambiar a la venta por numero
$("#cboBuscarPor").change(function () {
    if ($("#cboBuscarPor").val() == "fecha") {
        VISTA_BUSQUEDA["busquedaFecha"]()
    } else {
        VISTA_BUSQUEDA["busquedaVenta"]()
    }
})

//Mostrar el rango de fechas
$("#btnBuscar").click(function () {
    if ($("#cboBuscarPor").val() == "fecha") {

        //Si alguna de las fechas es igual a vacío se retormará u mensaje
        if ($("#txtFechaInicio").val().trim() == "" || $("#txtFechaFin").val().trim() == "") {
            toastr.warning("", "Debe ingresar fecha inicio y fin")
            return;
        } 
    } else {

        if ($("#txtNumeroVenta").val().trim() == "") {
            toastr.warning("", "Debe ingresar el numero de venta")
            return;
        }
    }

    let numeroVenta = $("#txtNumeroVenta").val()
    let fechaInicio = $("#txtFechaInicio").val()
    let fechaFin = $("#txtFechaFin").val()

    $(".card-body").find("div.row").LoadingOverlay("show");

    fetch(`/Venta/Historial?numeroVenta=${numeroVenta}&fechaInicio=${fechaInicio}&fechaFin=${fechaFin}`)
        .then(response => {

            $(".card-body").find("div.row").LoadingOverlay("hide");
            return response.ok ? response.json() : Promise.reject(response);
        })
        .then(responseJson => {
            //Limpiar el tbody


            $("#tbventa tbody").html("");
            if (responseJson.length > 0) {
                responseJson.forEach((venta) => {
                    $("#tbventa tbody").append(
                        $("<tr>").append(
                            $("<td>").text(venta.fechaRegistro),
                            $("<td>").text(venta.numeroVenta),
                            $("<td>").text(venta.tipoDocumentoVenta),
                            $("<td>").text(venta.documentoCliente),
                            $("<td>").text(venta.nombreCliente),
                            $("<td>").text(venta.total),
                            $("<td>").append(
                                $("<button>").addClass("btn btn-info btn-sm").append(
                                    $("<i>").addClass("fas fa-eye")
                                ).data("venta", venta)

                            )
                        )
                    )
                })
            }
        })
})

//BOTÓN PARA VISUALIZAR LOS DETALLES DE LA VENTA

$("#tbventa tbody").on("click", ".btn-info", function () {
    let d = $(this).data("venta")

    $("#txtFechaRegistro").val(d.fechaRegistro)
    $("#txtNumVenta").val(d.numeroVenta)
    $("#txtUsuarioRegistro").val(d.usuario)
    $("#txtTipoDocumento").val(d.tipoDocumentoVenta)
    $("#txtDocumentoCliente").val(d.documentoCliente)
    $("#txtNombreCliente").val(d.nombreCliente)
    $("#txtSubTotal").val(d.subTotal)
    $("#txtIGV").val(d.impuestoTotal)
    $("#txtTotal").val(d.total)

    $("#tbProductos tbody").html("");

    d.detalleVenta.forEach((item) => {

        $("#tbProductos tbody").append(
            $("<tr>").append(
                $("<td>").text(item.descripcionProducto),
                $("<td>").text(item.cantidad),
                $("<td>").text(item.precio),
                $("<td>").text(item.total),

            )
        )
    })

    //URL DEL BOTÓN
    $("#linkImprimir").attr("href", `/Venta/MostrarPDFVenta?numeroVenta=${d.numeroVenta}`)


    //Modal se pueda mostrar
    $("#modalData").modal("show");

})