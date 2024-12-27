$(document).ready(function () {
    // Función para manejar el botón de continuar
    $("#btn-continue").on("click", function () {
        $("#getInfo-article").css("display", "none");
        $("#uploadInfo-section").removeClass("d-none").css("display", "block");

        var video = $('#youtube-video');
        var videoSrc = video.attr('src');
        video.attr('src', videoSrc);

    });

    // Función para manejar el dropdown
    $(".dropdown").on("click", function () {
      
        var dpMenu = ".dropdown-menu";
        if ($(dpMenu).hasClass("d-none")) {
            $(dpMenu).removeClass("d-none").addClass("d-block");
        } else {
            $(dpMenu).removeClass("d-block").addClass("d-none");
        }
        
    });

    var lang = getCookie('language');
    if (lang) {
        changeLanguage(lang);
    }

    // Manejar el clic en los elementos del dropdown
    $('.dropdown-item').click(function () {
        var selectedLang = $(this).data('lang');
        setCookie('language', selectedLang, 30);
        console.log(selectedLang);
        changeLanguage(selectedLang);
    });

    // Manejar el cambio del archivo
    $('#fileUpload').on('change', function () {
        var fileInput = $('#fileUpload')[0];
        var file = fileInput.files[0];
        var maxSize = 5 * 1024 * 1024; // 5MB
        var lang = getCookie('language') || 'es';

        if (!file) {
            showMessage(lang, 'Por favor selecciona un archivo', 'Please select a file');
        } else {
            var fileName = file.name;
            var fileSize = file.size;

            if (!fileName.endsWith('.zip')) {
                showMessage(lang, 'Por favor selecciona un archivo .zip', 'Please select a .zip file');
            } else if (fileSize > maxSize) {
                showMessage(lang, 'El archivo debe ser menor a 5MB', 'The file must be less than 5MB');
            } else {
                $('#alert-box').removeClass('d-block').addClass('d-none');
                $('#submit-button').attr('disabled', false);
            }
        }
    });

    function setCookie(name, value, days) {
        var expires = "";
        if (days) {
            var date = new Date();
            date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
            expires = "; expires=" + date.toUTCString();
        }
        document.cookie = name + "=" + (value || "") + expires + "; path=/";
    }

    function getCookie(name) {
        var nameEQ = name + "=";
        var ca = document.cookie.split(';');
        for (var i = 0; i < ca.length; i++) {
            var c = ca[i];
            while (c.charAt(0) == ' ') c = c.substring(1, c.length);
            if (c.indexOf(nameEQ) == 0) return c.substring(nameEQ.length, c.length);
        }
        return null;
    }

    function changeLanguage(lang) {
        if (lang === 'es') {
            $('#dropdownMenuButton').html('<span class="flag-btn">🇪🇸</span> Español');
            $('#index').text("Indice");
            $('#more-projects').text("Mas proyectos");
            $('#more-info-link').text("Mas informacion");
            $('#greeting-title').text("Hola!");
            $('#lead-tutorial').text("Sigue este tutorial para obtener los datos y saber quién te sigue de vuelta");
            $('#nxt-btn').text("Continuar");
            $('#lead-results').text("Los usuarios que no te siguen de vuelta son:");
            $('#about-p').text("Aca puedes ver mis otras creaciones");
            $('#alert-box').text("Por favor selecciona un archivo valido");
            $('#upload-title').text("Sube tu archivo de informacion de seguidos y seguidores");
            $('#submit-button').text("Subir archivo");
            $('#results-h1').text("Resultados");
            $('#about-h1').text("Sobre otros productos");
        } else if (lang === 'en') {
            $('#dropdownMenuButton').html('<span class="flag-btn">🇬🇧</span> English');
            $('#index').text("Index");
            $('#more-projects').text("More projects");
            $('#more-info-link').text("More info");
            $('#greeting-title').text("Hello!");
            $('#lead-tutorial').text("Follow the tutorial to get the data and know who doesn't follow you back");
            $('#nxt-btn').text("Continue");
            $('#lead-results').text("These are the users who don't follow you back:");
            $('#about-p').text("Here you can see my other creations");
            $('#alert-box').text("Please select a valid file");
            $('#upload-title').text("Upload your followers/following information file");
            $('#submit-button').text("Upload file");
            $('#results-h1').text("Results");
            $('#about-h1').text("Check my other projects");

        }
    }

    function showMessage(lang, msgEs, msgEn) {
        if (lang === 'es') {
            $('#alert-box').removeClass('d-none').addClass('d-block').text(msgEs);
        } else {
            $('#alert-box').removeClass('d-none').addClass('d-block').text(msgEn);
        }
        $('#submit-button').attr('disabled', true);
    }
});
