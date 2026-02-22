# Mods-Broforce Makefile

PROJECT_NAME := Mods-Broforce

# Custom build - no solution file, build individual projects
CUSTOM_BUILD := 1

# Custom help text for individual projects
define EXTRA_HELP
	@echo "Individual projects:"
	@echo "  make skipintro          make filteredbros"
	@echo "  make expendabros        make 007-patch"
	@echo "  make dressermod"
endef
export EXTRA_HELP

include Scripts/Makefile.common

# Build all updated projects
.PHONY: build
build: skipintro expendabros filteredbros 007-patch dressermod

.PHONY: build-no-launch
build-no-launch:
	$(MAKE) skipintro LAUNCH=no
	$(MAKE) expendabros LAUNCH=no
	$(MAKE) filteredbros LAUNCH=no
	$(MAKE) 007-patch LAUNCH=no
	$(MAKE) dressermod LAUNCH=no

.PHONY: clean
clean:
	"$(MSBUILD)" "SkipIntro/src/SkipIntroMod.csproj" /t:Clean $(MSBUILD_FLAGS)
	"$(MSBUILD)" "ExpendablesBrosInGame/src/ExpendablesBrosInGame.csproj" /t:Clean $(MSBUILD_FLAGS)
	"$(MSBUILD)" "FilteredBros/src/FilteredBrosMod.csproj" /t:Clean $(MSBUILD_FLAGS)
	"$(MSBUILD)" "007_Patch/src/DoubleBroSevenTrained.csproj" /t:Clean $(MSBUILD_FLAGS)
	"$(MSBUILD)" "DresserMod/src/DresserMod/DresserMod.csproj" /t:Clean $(MSBUILD_FLAGS)

.PHONY: rebuild
rebuild: clean build-no-launch

# Individual project targets
.PHONY: skipintro
skipintro:
	"$(MSBUILD)" "SkipIntro/src/SkipIntroMod.csproj" $(MSBUILD_FLAGS) $(LAUNCH_FLAGS)

.PHONY: expendabros
expendabros:
	"$(MSBUILD)" "ExpendablesBrosInGame/src/ExpendablesBrosInGame.csproj" $(MSBUILD_FLAGS) $(LAUNCH_FLAGS)

.PHONY: filteredbros
filteredbros:
	"$(MSBUILD)" "FilteredBros/src/FilteredBrosMod.csproj" $(MSBUILD_FLAGS) $(LAUNCH_FLAGS)

.PHONY: 007-patch
007-patch:
	"$(MSBUILD)" "007_Patch/src/DoubleBroSevenTrained.csproj" $(MSBUILD_FLAGS) $(LAUNCH_FLAGS)

.PHONY: dressermod
dressermod:
	"$(MSBUILD)" "DresserMod/src/DresserMod/DresserMod.csproj" $(MSBUILD_FLAGS) $(LAUNCH_FLAGS)
