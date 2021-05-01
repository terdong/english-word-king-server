* nuget package
** 패키지 관리자 콘솔창을 연 다음.

1. Install-Package Newtonsoft.Json
(json 패키지)

2. Install-Package linq2db.PostgreSQL
(linq to postgresql패키지)

3. Install-Package mongocsharpdriver
(mongoDB 패키지)

4. Install-Package protobuf-net
(protobuf for c# 패키지)

5. Install-Package Google.Apis
(Google id token 인증을 위한 패키지)

6. Install-Package YamlDotNet

7. Install-Package StackExchange.Redis

8. Install-Package NUnit
(유닛테스트를 위함)
Install-Package NUnitTestAdapter
Install-Package NUnit.Runners

* postgresql
database : ewk
id : ewk_admin
password : $K1JfI9Ojd+3_l

** .sql 복구방법
psql 실행 후 postgresql=# \i c:/temp/temp.sql
(\i /path)

* mongodb
** 실행 커맨드
cmd> mongod --dbpath d:\mongodb\data\db

* mongodb & redis cmd prompt
@echo off
start /b mongod --dbpath C:\Users\terdong\Documents\MongoDbData
start /d "C:\Users\terdong\Documents\GitHub\redis\msvs\x64\Release\" redis-server.exe
pause