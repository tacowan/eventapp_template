Modifying the WP8 Visual Studio Project
=======================================

1.	Change App.mawssubdomain to your Windows Azure Web Site name.  (not the full domain, just the first part before azurewebsites.net.  This is in the file called App.xaml.cs.
2.	Change App.MY_BLOB_CONTAINER to the full public path to your events blob container.  It’ll be in the format of http://youraccount.blob.core.windows.net/events/. 

## Create cached/scheduled feeds
Now we are going to create 5 Windows Azure Scheduled events using the scheduler.  
Each of these should be configured to run at least once a day.  You can easily see within the MainPage.xaml.cs file how these blobs will provide data for the panorama page.  You can also verify you are getting data by changing “cache” to “eventful” to see the results returned to your browser if you like.

1.  Create the music feed.  Create an HTTP GET event using the scheduler service using your own azure website but following this format.  Make sure you change “location=austin” to the city you are targeting for your app. 

   http://YOURSITENAME.azurewebsites.net/cache/rest/events/search?location=austin&sort_order=popularity&page_size=50&category=music&blob_name=music 
2.  Create the family feed

   http://YOURSITENAME.azurewebsites.net/cache/rest/events/search?location=austin&sort_order=popularity&page_size=50&category=family_fun_kids&blobname=family_fun_kids 
3.  Create the sports feed

   http://YOURSITENAME.azurewebsites.net/cache/rest/events/search?location=austin&sort_order=popularity&page_size=50&category=sports&blobname=sports 
4.  Create the performing arts feed.

   http://YOURSITENAME.azurewebsites.net/cache/rest/events/search?location=austin&sort_order=popularity&page_size=50&category=performing_arts&blobname=performing_arts 
5.  Create the venues feed.  Notice that this one is slightly different and uses a different eventful rest call for the venues.  As before change the city name.  You may also modify the page size to get more or less items to show on the panorama.

   http://YOURSITENAME.azurewebsites.net/cache/rest/venues/search?sort_order=popularity&location=austin&page_size=20&blobname=topvenues 

