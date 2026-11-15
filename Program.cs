
using Microsoft.Data.SqlClient;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

var apiKey = builder.Configuration["ApiKey"];

const string Conn = @"Server=(localdb)\MSSQLLocalDB;" +
                    "Database=Guestbook;Integrated Security=True";

app.MapGet("/search", (string q) =>
{
    using var db = new SqlConnection(Conn);
    db.Open();

    var cmd = new SqlCommand(
        "SELECT Name, Msg FROM Entries WHERE Msg LIKE @q", db);

    cmd.Parameters.AddWithValue("@q", "%" + q + "%");

    using var r = cmd.ExecuteReader();

    var html = "";

    while (r.Read())
    {
        html += "<p>" +
            WebUtility.HtmlEncode($"{r[0]}: {r[1]}") +
            "</p>";
    }

    return Results.Content(html, "text/html");
});

app.Run();
