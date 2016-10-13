/// <reference path="/Assets/admin/libs/angular/angular.js" />

(function () {
    angular.module('myshop.contact_details', ['myshop.common']).config(config);

    config.$inject = ['$stateProvider', '$urlRouterProvider'];

    function config($stateProvider, $urlRouterProvider) {

        $stateProvider.state('contact_details', {
            url: "/contact_details",
            parent: 'base',
            templateUrl: "/app/shared/contact_details/contactDetailListView.html",
            controller: "contactDetailListController"
        })
            .state('add_contact_detail', {
                url: "/add_contact_detail",
                parent: 'base',
                templateUrl: "/app/shared/contact_details/contactDetailAddView.html",
                controller: "contactDetailAddController"
            })
            .state('edit_contact_detail', {
                url: "/edit_contact_detail/:id",
                parent: 'base',
                templateUrl: "/app/shared/contact_details/contactDetailEditView.html",
                controller: "contactDetailEditController"
            });
    }
})();