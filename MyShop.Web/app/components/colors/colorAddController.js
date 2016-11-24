(function (app) {
    app.controller('colorAddController', colorAddController);

    colorAddController.$inject = ['apiService', '$scope', 'notificationService', '$state', '$location', 'commonService'];

    function colorAddController(apiService, $scope, notificationService, $state, $location, commonService) {
        $scope.color = {
        }

        $scope.AddColor = AddColor;

        function AddColor() {
            apiService.post('api/color/create', $scope.color, addSuccessed, addFailed);
        }
        function addSuccessed() {
            notificationService.displaySuccess($scope.color.Name + ' đã được thêm mới.');
            $state.go('colors');
        }
        function addFailed(response) {
            notificationService.displayError(response.data.Message);
            notificationService.displayErrorValidation(response);
        }
    }

})(angular.module('myshop.colors'));