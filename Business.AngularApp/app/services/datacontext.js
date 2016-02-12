(function () {
    'use strict';

    var serviceId = 'datacontext';
    angular.module('app').
        factory(serviceId, ['common', '$http', datacontext]);

    function datacontext(common, $http) {
        var $q = common.$q;

        var service = {
            getEmployees: getEmployees,
            getEmployee: getEmployee,
            saveEmployee: saveEmployee,
            deleteEmployee: deleteEmployee,
            updateEmployee: updateEmployee,
            getEmployeeCount: getEmployeeCount
        };

        return service;

        function getMessageCount() { return $q.when(72); }

        function getPeople() {
            var people = [
                { firstName: 'John', lastName: 'Papa', age: 25, location: 'Florida' },
                { firstName: 'Ward', lastName: 'Bell', age: 31, location: 'California' },
                { firstName: 'Colleen', lastName: 'Jones', age: 21, location: 'New York' },
                { firstName: 'Madelyn', lastName: 'Green', age: 18, location: 'North Dakota' },
                { firstName: 'Ella', lastName: 'Jobs', age: 18, location: 'South Dakota' },
                { firstName: 'Landon', lastName: 'Gates', age: 11, location: 'South Carolina' },
                { firstName: 'Haley', lastName: 'Guthrie', age: 35, location: 'Wyoming' }
            ];
            return $q.when(people);
        }

        function getEmployees() {

            return $http.get("http://localhost:57148/api/employee");
        }

        function getEmployee(EmpNo) {
           
            return $http.get("http://localhost:57148/api/employee/" + EmpNo);
        }

        function getEmployeeCount() {
    
            return $http.get("http://localhost:57148/api/employee/" + EmpNo);
        }

        function saveEmployee(Employee) {

            var request = $http({
                method: "post",
                url: "http://localhost:57148/api/employee",
                data: Employee
            });
            return request
        }

        function deleteEmployee(EmpNo) {

            var request = $http({
                method: "delete",
                url: "http://localhost:57148/api/employee/" + EmpNo
            });
            return request;
        }

        function updateEmployee(EmpNo, Employee) {
            
            var request = $http({
                method: "put",
                url: "http://localhost:57148/api/employee/" + EmpNo,
                data: Employee
            });
            return request;
        }
    }
})();