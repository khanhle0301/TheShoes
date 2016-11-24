(function (app) {
    app.controller('heelEditController', heelEditController);

    heelEditController.$inject = ['apiService', '$scope', 'notificationService', '$state', 'commonService', '$stateParams'];

    function heelEditController(apiService, $scope, notificationService, $state, commonService, $stateParams) {
        $scope.heel = {};
        $scope.UpdateHeel = UpdateHeel;

        function loadHeelDetail() {
            apiService.get('api/heel/getbyid/' + $stateParams.id, null, function (result) {
                $scope.heel = result.data;
            }, function (error) {
                notificationService.displayError(error.data);
            });
        }
        function UpdateHeel() {
            apiService.put('api/heel/update', $scope.heel, updateSuccessed, addFailed);
        }
        function updateSuccessed() {
            notificationService.displaySuccess($scope.heel.Name + ' đã được cập nhật.');
            $state.go('heels');
        }
        function addFailed(response) {
            notificationService.displayError(response.data.Message);
            notificationService.displayErrorValidation(response);
        }

        loadHeelDetail();
    }

})(angular.module('myshop.heels'));