* nuget package
** ��Ű�� ������ �ܼ�â�� �� ����.

1. Install-Package Newtonsoft.Json
(json ��Ű��)

2. Install-Package linq2db.PostgreSQL
(linq to postgresql��Ű��)

3. Install-Package mongocsharpdriver
(mongoDB ��Ű��)

4. Install-Package protobuf-net
(protobuf for c# ��Ű��)

5. Install-Package Google.Apis
(Google id token ������ ���� ��Ű��)

6. Install-Package YamlDotNet

7. Install-Package StackExchange.Redis

8. Install-Package NUnit
(�����׽�Ʈ�� ����)
Install-Package NUnitTestAdapter
Install-Package NUnit.Runners

* postgresql
database : ewk
id : ewk_admin
password : $K1JfI9Ojd+3_l

** .sql �������
psql ���� �� postgresql=# \i c:/temp/temp.sql
(\i /path)

* mongodb
** ���� Ŀ�ǵ�
cmd> mongod --dbpath d:\mongodb\data\db

* mongodb & redis cmd prompt
@echo off
start /b mongod --dbpath C:\Users\terdong\Documents\MongoDbData
start /d "C:\Users\terdong\Documents\GitHub\redis\msvs\x64\Release\" redis-server.exe
pause