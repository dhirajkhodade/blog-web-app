$(document).ready(function () {
    $("#postSearch").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/Home/SearchPosts",
                type: "POST",
                dataType: "json",
                data: { searchKeyword: request.term },
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.title, value: item.title, id: item.id };
                    }))

                }
            })
        },
        select: function (event, ui) {
            redirect(ui.item.id);
        },
        messages: {
            noResults: "", results: function (resultsCount) { }
        }
    });
})

function redirect(id) {
    window.location.href = '/Blog/PostDetails/' + id;
}

function FetchRecentPosts() {
    $.ajax({
        url: '@Url.Action("GetRecentPosts", "Home")',
        success: function (data) {
            $("#recent-posts-component").html(data);
        }
    })
}
