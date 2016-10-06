(function (app) {
    app.controller('providerAddController', providerAddController);

    providerAddController.$inject = ['apiService', '$scope', 'notificationService', '$state', 'commonService'];

    function providerAddController(apiService, $scope, notificationService, $state, commonService) {
        $scope.provider = {
            CreatedDate: new Date(),
            Status: true
        }

        $scope.GetSeoTitle = GetSeoTitle;

        function GetSeoTitle() {
            $scope.provider.Alias = commonService.getSeoTitle($scope.provider.Name);
        }

        $scope.AddProvider = AddProvider;

        function AddProvider() {
            apiService.post('api/provider/create', $scope.provider,
                function (result) {
                    notificationService.displaySuccess(result.data.Name + ' đã được thêm mới.');
                    $state.go('providers');
                }, function (error) {
                    notificationService.displayError('Thêm mới không thành công.');
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
    }

})(angular.module('myshop.providers'));