local fs = require("@lune/fs")
local process = require("@lune/process")
local Semver = require("../luau_packages/semver")

local LOCAL_APP_DATA = process.env["LOCALAPPDATA"]
local GALE_LOCATION = LOCAL_APP_DATA .. "/Gale/gale.exe"

local ARTIFACTS_DIRECTORY = process.cwd .. "artifacts"
local THUNDERSTORE_ARTIFACTS_DIRECTORY = ARTIFACTS_DIRECTORY .. "/thunderstore"

type PackageMetadata = {
	team_name: string,
	package_name: string,
	version: any,
}

local possible_versions: { PackageMetadata } = {}

for index, file in fs.readDir(THUNDERSTORE_ARTIFACTS_DIRECTORY) do
	local components = file:split("-")
	local team_name, package_name, version_plus_extension = table.unpack(components)
	local version = version_plus_extension:sub(1, -5) -- Why -5 instead of -4? no fucking idea probably a null byte
	local semver_version = Semver.parse(version):unwrap()

	local package: PackageMetadata = {
		team_name = team_name,
		package_name = package_name,
		version = semver_version,
	}
	possible_versions[index] = package
end


local highest_version_package = nil

for i = 1, #possible_versions, 2 do
	local package1 = possible_versions[i]
	local package2 = possible_versions[i + 1]

	local current_highest = nil

	if package2 then
		-- Compare the two current packages
		if package1.version > package2.version then
			current_highest = package1
		else
			current_highest = package2
		end
	else
		-- If there's an odd number of packages, the last one is processed alone
		current_highest = package1
	end

	-- Compare with overall highest version
	if highest_version_package == nil or current_highest.version > highest_version_package.version then
		highest_version_package = current_highest
	end
end

if highest_version_package then
	local original_file_name =
		`{highest_version_package.team_name}-{highest_version_package.package_name}-{highest_version_package.version}.zip`
	local absolute_path = THUNDERSTORE_ARTIFACTS_DIRECTORY .. `/{original_file_name}`
	print(absolute_path)

	local result = process.create(GALE_LOCATION, { "--install", absolute_path })
	local status = result:status()

	if status.ok then
		print("Mod imported and gale opened!")
	else
		print(result.stderr:readToEnd())
		process.exit(status.code)
	end
else
	print("No packages found")
end
