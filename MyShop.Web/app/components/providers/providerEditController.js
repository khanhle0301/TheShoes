(function (app) {
    app.controller('providerEditController', providerEditController);

    providerEditController.$inject = ['apiService', '$scope', 'notificationService', '$state', '$stateParams', 'commonService'];

    function providerEditController(apiService, $scope, notificationService, $state, $stateParams, commonService) {
        $scope.provider = {            
            Status: true
        }

        $scope.UpdateProvider = UpdateProvider;
        $scope.GetSeoTitle = GetSeoTitle;

        function GetSeoTitle() {
            $scope.provider.Alias = commonService.getSeoTitle($scope.provider.Name);
        }

        function loadProviderDetail() {
            apiService.get('api/provider/getbyid/' + $stateParams.id, null, function (result) {
                $scope.provider = result.data;
            }, function (error) {
                notificationService.displayError(error.data);
            });
        }

        $scope.ChooseImage = function () {
            var finder = new CKFinder();
            finder.selectActionFunction = function (fileUrl) {
                $scope.$apply(function () {
                    $scope.provider.Image = fileUrl;
                })
            }
            finder.popup();
        }

        function UpdateProvider() {
            apiService.put('api/provider/update', $scope.provider,
                function (result) {
                    notificationService.displaySuccess(result.data.Name + ' đã được cập nhật.');
                    $state.go('providers');
                }, function (error) {
                    notificationService.displayError('Cập nhật không thành công.');
                });
        }
       
        loadProviderDetail();
    }

})(angular.module('myshop.providers'));