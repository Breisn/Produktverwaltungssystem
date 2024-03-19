$(document).ready(function () {
    updateUIBasedOnAuthStatus();

    //Auth-Token
    function updateUIBasedOnAuthStatus() {
        const isLoggedIn = localStorage.getItem("authToken") !== null;
        toggleAuthenticationButtons(isLoggedIn);
    }

    var notyf = new Notyf();
    var failednotyf = new Notyf({
        duration: 4000,
        position: {
            x: 'center',
            y: 'top'
        },
        dismissible: true
    });

    // Registrierungs-Event
    $(document).on('submit', '#registerForm', function (event) {
        event.preventDefault();
        var formData = {
            Email: $("#registerEmail").val(),
            Benutzername: $("#registerUsername").val(),
            Passwort: $("#registerPassword").val(),
            PasswortBestaetigen: $("#confirmPassword").val()
        };

        $.ajax({
            url: "/api/Account/Register",
            type: "POST",
            contentType: 'application/json',
            data: JSON.stringify(formData),
            success: function (response) {
                notyf.success("Registrierung erfolgreich");
                $('#registerModal').modal('hide');
                saveAuthStatus(response.token); // Angenommen, der Server gibt ein Token zurück
                updateUIBasedOnAuthStatus();
            },
            error: function (xhr) {
                var response = JSON.parse(xhr.responseText);
                var errorMessage = response.message || "Ein unbekannter Fehler ist aufgetreten.";
                failednotyf.error(errorMessage);
            }
        });
    });

    // Login-Event
    $(document).on('submit', '#loginForm', function (event) {
        event.preventDefault();
        var formData = {
            LoginCredential: $("#loginEmail").val(),
            Passwort: $("#loginPassword").val()
        };

        $.ajax({
            url: "/api/Account/Login",
            type: "POST",
            contentType: 'application/json',
            data: JSON.stringify(formData),
            success: function (response) {
                notyf.success('Login erfolgreich');
                $('#loginModal').modal('hide');
                saveAuthStatus(response.token);
                updateUIBasedOnAuthStatus();
            },
            error: function (xhr) {
                var response = JSON.parse(xhr.responseText);
                var errorMessage = response.message || "Ein unbekannter Fehler ist aufgetreten.";
                failednotyf.error(errorMessage);
            }
        });
    });

    // Logout-Event
    $('#logoutButton').click(function () {
        localStorage.removeItem("authToken");
        notyf.success('Logout erfolgreich');

        setTimeout(function () {
            updateUIBasedOnAuthStatus();
            window.location.href = "/";
        }, 1000);
    });

    function saveAuthStatus(token) {
        localStorage.setItem("authToken", token);
    }

    function toggleAuthenticationButtons(isAuthenticated) {
        if (isAuthenticated) {
            $('#loginButton, #registerButton').hide();
            $('#profileButton, #logoutButton').show();
        } else {
            $('#loginButton, #registerButton').show();
            $('#profileButton, #logoutButton').hide();
        }
    }

    //Search
    $(document).ready(function () {
        $(document).on('submit', '#searchForm', function (event) {
            event.preventDefault();
            var formData = $(this).serialize();

            $.ajax({
                url: '/Home/Index', 
                type: 'GET',
                data: formData,
                success: function (data) {
                    $('#productsList').html(data);
                },
                error: function (xhr, status, error) {
                    alert("Ein Fehler ist aufgetreten: " + xhr.status + " " + error);
                }
            });
        });
    });
});