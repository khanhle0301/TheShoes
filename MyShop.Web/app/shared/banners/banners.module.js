/// <reference path="/Assets/admin/libs/angular/angular.js" />

(function () {
    angular.module('myshop.banners', ['myshop.common']).config(config);

    config.$inject = ['$stateProvider', '$urlRouterProvider'];

    function config($stateProvider, $urlRouterProvider) {

        $stateProvider.state('banners', {
            url: "/banners",
            templateUrl: "/app/shared/banners/bannerListView.html",
            controller: "bannerListController"
        })
            .state('banner_add', {
                url: "/banner_add",
                templateUrl: "/app/shared/banners/bannerAddView.html",
                controller: "bannerAddController"
            })
            .state('banner_edit', {
                url: "/banner_edit/:id",
                templateUrl: "/app/shared/banners/bannerEditView.html",
                controller: "bannerEditController"
            });
    }
})();