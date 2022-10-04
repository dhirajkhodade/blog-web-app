function fetchRecentPosts() {
    $.ajax({
        url: '/Home/GetRecentPosts',
        success: function (data) {
            $("#recent-posts-component").html(data);
        }
    })
}


var connection = new signalR.HubConnectionBuilder().withUrl("/notificationHub").build();
connection.start();
connection.on("PostPublish", function (msg) {
    fetchRecentPosts()
    $.notify(msg, {
        position: "bottom right",
        className: 'success',
        autoHideDelay : 5000
    });
});

connection.on("PostUnPublish", function () {
    fetchRecentPosts()
});

