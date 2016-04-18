$('#modalUploader').on('show.bs.modal', function (event) {
    var uploader = new qq.FileUploaderBasic({
        element: document.getElementById('file-uploader-demo1'),
        button: document.getElementById('areaSubir'),
        action: '/Files/Upload',
        params: { ruta: $('#RutaActual').val() },
        allowedExtensions: ['jpg', 'jpeg', 'png', 'gif', 'doc', 'docx', 'pdf'],
        multiple: false,
        onComplete: function (id, fileName, responseJSON) {
            $.gritter.add({
                title: 'Upload file',
                text: responseJSON.message
            });

        },
        onSubmit: function (id, fileName) {
            var strHtml = "<li>" + fileName + "</li>";
            $("#dvxArchivos").append(strHtml);

        }
    });
});

$('#modalUploader').on('hidden.bs.modal', function () {
    RecargarRuta();

})

function AbrirCarpeta(pCarpeta) {
    $("#RutaSolicitada").val(pCarpeta);
    $("#frmMain").submit();

}
function AbrirRuta(pCarpeta) {
    $("#RutaSolicitada").val(pCarpeta);
    $("#frmMain").submit();
}

function RecargarRuta() {
    AbrirCarpeta($("#RutaActual").val());
}

function EliminarArchivo(pRuta) {

    if (confirm("Are you sure that want to delete this file?")) {
        $.gritter.add({
            title: 'Delete file',
            text: "File deleted"
        });
        $("#ArchivoParaEliminar").val(pRuta);
        AbrirCarpeta($("#RutaActual").val());

    }
}

function SubirNivel() {
    if ($("#TieneSuperior").val() == "1") {
        var strRutaAnterior = $("#RutaSuperior").val();
        AbrirRuta(strRutaAnterior);

    }
}
function IrInicio() {
    AbrirRuta($("#RutaRaiz").val());
}

function AbrirDialogoArchivo() {


    $("#modalUploader").modal();
}


function getParameterByName(name) {
    name = name.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
    var regexS = "[\\?&]" + name + "=([^&#]*)";
    var regex = new RegExp(regexS);
    var results = regex.exec(window.location.search);
    if (results == null)
        return "";
    else
        return decodeURIComponent(results[1].replace(/\+/g, " "));
}
