curl --location --request POST 'https://localhost:5001/api/login' --header 'Content-Type: application/json' --data-raw '{
"UserName":"Vijay",
"Password":"vijay"
}'

curl --location --request GET 'https://localhost:5001/api/values'
--header 'Authorization: bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJWaWpheSBEb24iLCJlbWFpbCI6InZkb250aGlyZWRkeUBnbWFpbC5jb20iLCJEYXRlT2ZKb2luZyI6IjIwMTAtMDgtMDIiLCJqdGkiOiIzY2FjY2QyMy1lODY0LTQwMWQtYWI0NS1iZTc3YzBjZTM0NDgiLCJleHAiOjE1Nzg4ODE3NTgsImlzcyI6InNlcnZpY2VjaGFubmVsLmNvbSIsImF1ZCI6InNlcnZpY2VjaGFubmVsLmNvbSJ9.AxmTOLPEG1n2uqHw6EFqOOS82JQjjQfqD0ZRVDrnhVI'
