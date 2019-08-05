$(document).ready(function () {
    $("#LeagueId").change(function () {
        $("#TeamId").empty();
        $.ajax({
            type: 'POST',
            url: URL,
            dataType: 'json',
            data: { leagueId: $("#LeagueId").val() },
            success: function (teams) {
                $.each(teams, function (i, team) {
                    $("#TeamId").append('<option value="'
                        + team.TeamId + '">'
                        + team.Name + '</option>');
                });
            },
            error: function (ex) {
                alert('Error al recuperar los equipos.' + ex);
            }
        });
        return false;
    })
});