/// <reference path="/Assets/admin/libs/angular/angular.js" />

(function () {
    angular.module('myshop.posts', ['myshop.common']).config(config);

    config.$inject = ['$stateProvider', '$urlRouterProvider'];

    function config($stateProvider, $urlRouterProvider) {
        $stateProvider.state('posts', {
            url: "/posts",
            templateUrl: "/app/components/posts/postListView.html",
            controller: "postListController"
        }).state('post_add', {
            url: "/post_add",
            templateUrl: "/app/components/posts/postAddView.html",
            controller: "postAddController"
        }).state('post_edit', {
            url: "/post_edit/:id",
            templateUrl: "/app/components/posts/postEditView.html",
            controller: "postEditController"
        });
    }
})();