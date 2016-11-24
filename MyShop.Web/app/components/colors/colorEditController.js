(function (app) {
    app.controller('colorEditController', colorEditController);

    colorEditController.$inject = ['apiService', '$scope', 'notificationService', '$state', 'commonService', '$stateParams'];

    function colorEditController(apiService, $scope, notificationService, $state, commonService, $stateParams) {
        $scope.color = {};
        $scope.UpdateColor = UpdateColor;

        function loadColorDetail() {
            apiService.get('api/color/getbyid/' + $stateParams.id, null, function (result) {
                $scope.color = result.data;
            }, function (error) {
                notificationService.displayError(error.data);
            });
        }
        function UpdateColor() {
            apiService.put('api/color/update', $scope.color, updateSuccessed, addFailed);
        }
        function updateSuccessed() {
            notificationService.displaySuccess($scope.color.Name + ' đã được cập nhật.');
            $state.go('colors');
        }
        function addFailed(response) {
            notificationService.displayError(response.data.Message);
            notificationService.displayErrorValidation(response);
        }

        loadColorDetail();
    }

})(angular.module('myshop.colors'));