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

connection.on("PostViewed", function (msg) {
    var anchors = document.querySelectorAll("#post-views-" + msg.postid);
    anchors.forEach(function (a) {
        a.text = msg.views;
    });

    var tds = document.querySelectorAll("td#post-views-" + msg.postid);
    tds.forEach(function (td) {
        td.innerHTML = msg.views;
    });

});



