/// <reference path="/Assets/admin/libs/angular/angular.js" />

(function () {
    angular.module('myshop.heels', ['myshop.common']).config(config);

    config.$inject = ['$stateProvider', '$urlRouterProvider'];

    function config($stateProvider, $urlRouterProvider) {

        $stateProvider.state('heels', {
            url: "/heels",
            parent: 'base',
            templateUrl: "/app/components/heels/heelListView.html",
            controller: "heelListController"
        })
            .state('heel_add', {
                url: "/heel_add",
                parent: 'base',
                templateUrl: "/app/components/heels/heelAddView.html",
                controller: "heelAddController"
            })
            .state('heel_edit', {
                url: "/heel_edit/:id",
                parent: 'base',
                templateUrl: "/app/components/heels/heelEditView.html",
                controller: "heelEditController"
            });
    }
})();