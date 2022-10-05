# GeekSpot - Blogging Web App
This is simple blogging web app which allows users to write and publish articles.

__You can run this app with just F5 in VS2022 or with dotnet run command.__
__username = dhiraj__
__password = password__

# Features 
- __Landing page__ 
	- Featuring top 4 most viewed popular articles on top showing 1st image from that article. 
	- List of all the published articles with its short summary and total views (which updates in realtime), Author name and published date.
	- Recently published 3 articles on the right side which updates in realtime without refreshing user's webpage.
	- Search functionlaity to search published posts by title.
	- Tag cloud - where you can click on a Tag to get all the posts tagged under that tag.
- __Publisher dashboard__
	- This is authenticated page user needs to login to get access to this page.
	- Publisher can see list of all the published and unpublished articles on this page.
	- Article views which updates in realtime when some user view that article. 
	- Publisher can create and save article without publishing it righaway. 
	- Edit and create functinality where user can edit or create new article with fully functional TinyMCE rich text editor.
	- User can publish or unpublish the articles from this dashboard
- __Realtime notification (Make sure you open the app in at least two browser tabs to see realtime notifications in action)__
	- When users view the article, view count for that article gets updated in realtime to all the other online users.
	- When publisher publishes new article all the online users gets notification popup in realtime (on right bottom corner of browser).
	- When publisher publishes new article the recent posts section gets updated in realtime for all the online users without refreshing the webpage.


# Technical details
- Written in Asp.Net MVC with .Net 6 
- Used Sqlite local database as data storage with EFCore 6 ORM
- Used Cookie based authentication to authenticate publisher
- Used Repository pattern to make code decoupled and testable
- Added few unit tests using XUnit to makesure controller and repository are testable.
- Used SignalR for realtime notification features
- Used TinyMCE WYSIWYG rich text editor
