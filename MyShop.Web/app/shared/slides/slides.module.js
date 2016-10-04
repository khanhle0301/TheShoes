/// <reference path="/Assets/admin/libs/angular/angular.js" />

(function () {
    angular.module('myshop.slides', ['myshop.common']).config(config);

    config.$inject = ['$stateProvider', '$urlRouterProvider'];

    function config($stateProvider, $urlRouterProvider) {

        $stateProvider.state('slides', {
            url: "/slides",
            templateUrl: "/app/shared/slides/slideListView.html",
            controller: "slideListController"
        })
            .state('slide_add', {
                url: "/slide_add",
                templateUrl: "/app/shared/slides/slideAddView.html",
                controller: "slideAddController"
            })
            .state('slide_edit', {
                url: "/slide_edit/:id",
                templateUrl: "/app/shared/slides/slideEditView.html",
                controller: "slideEditController"
            });
    }
})();