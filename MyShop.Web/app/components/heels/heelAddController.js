(function (app) {
    app.controller('heelAddController', heelAddController);

    heelAddController.$inject = ['apiService', '$scope', 'notificationService', '$state', '$location', 'commonService'];

    function heelAddController(apiService, $scope, notificationService, $state, $location, commonService) {
        $scope.heel = {
        }

        $scope.AddHeel = AddHeel;

        function AddHeel() {
            apiService.post('api/heel/create', $scope.heel, addSuccessed, addFailed);
        }
        function addSuccessed() {
            notificationService.displaySuccess($scope.heel.Name + ' đã được thêm mới.');
            $state.go('heels');
        }
        function addFailed(response) {
            notificationService.displayError(response.data.Message);
            notificationService.displayErrorValidation(response);
        }
    }

})(angular.module('myshop.heels'));