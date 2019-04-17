var app = angular.module("MainApp", ['ui.router','ngFileSaver']);

app.run(function ($rootScope, $state,$window,$http) {

  // Sends this header with any AJAX request
  $http.defaults.headers.common['Access-Control-Allow-Origin'] = '*';
  // Send this header only in post requests. Specifies you are sending a JSON object
  $http.defaults.headers.post['dataType'] = 'json'
  
        $window.onload = function() {

        }

    }
);