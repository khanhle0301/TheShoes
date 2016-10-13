/// <reference path="/Assets/admin/libs/angular/angular.js" />

(function () {
    angular.module('myshop.feedbacks', ['myshop.common']).config(config);

    config.$inject = ['$stateProvider', '$urlRouterProvider'];

    function config($stateProvider, $urlRouterProvider) {
        $stateProvider.state('feedbacks', {
            url: "/feedbacks",
            parent: 'base',
            templateUrl: "/app/components/feedbacks/feedbackListView.html",
            controller: "feedbackListController"
        }).state('feedback_add', {
            url: "/feedback_add",
            parent: 'base',
            templateUrl: "/app/components/feedbacks/feedbackAddView.html",
            controller: "feedbackAddController"
        }).state('feedback_edit', {
            url: "/feedback_edit/:id",
            parent: 'base',
            templateUrl: "/app/components/feedbacks/feedbackEditView.html",
            controller: "feedbackEditController"
        });
    }
})();