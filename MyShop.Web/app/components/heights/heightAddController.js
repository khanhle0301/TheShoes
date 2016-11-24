(function (app) {
    app.controller('heightAddController', heightAddController);

    heightAddController.$inject = ['apiService', '$scope', 'notificationService', '$state', '$location', 'commonService'];

    function heightAddController(apiService, $scope, notificationService, $state, $location, commonService) {
        $scope.height = {
        }

        $scope.AddHeight = AddHeight;

        function AddHeight() {
            apiService.post('api/height/create', $scope.height, addSuccessed, addFailed);
        }
        function addSuccessed() {
            notificationService.displaySuccess($scope.height.Name + ' đã được thêm mới.');
            $state.go('heights');
        }
        function addFailed(response) {
            notificationService.displayError(response.data.Message);
            notificationService.displayErrorValidation(response);
        }
    }

})(angular.module('myshop.heights'));