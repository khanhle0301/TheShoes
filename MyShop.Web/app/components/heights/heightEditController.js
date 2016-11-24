(function (app) {
    app.controller('heightEditController', heightEditController);

    heightEditController.$inject = ['apiService', '$scope', 'notificationService', '$state', 'commonService', '$stateParams'];

    function heightEditController(apiService, $scope, notificationService, $state, commonService, $stateParams) {
        $scope.height = {};
        $scope.UpdateHeight = UpdateHeight;

        function loadHeightDetail() {
            apiService.get('api/height/getbyid/' + $stateParams.id, null, function (result) {
                $scope.height = result.data;
            }, function (error) {
                notificationService.displayError(error.data);
            });
        }
        function UpdateHeight() {
            apiService.put('api/height/update', $scope.height, updateSuccessed, addFailed);
        }
        function updateSuccessed() {
            notificationService.displaySuccess($scope.height.Name + ' đã được cập nhật.');
            $state.go('heights');
        }
        function addFailed(response) {
            notificationService.displayError(response.data.Message);
            notificationService.displayErrorValidation(response);
        }

        loadHeightDetail();
    }

})(angular.module('myshop.heights'));