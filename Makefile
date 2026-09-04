# Mods-Broforce Makefile

PROJECT_NAME := Mods-Broforce

PROJECTS := \
  skipintro=SkipIntro/SkipIntroMod.csproj \
  expendabros=ExpendablesBrosInGame/ExpendablesBrosInGame.csproj \
  filteredbros=FilteredBros/FilteredBrosMod.csproj \
  007-patch=007_Patch/DoubleBroSevenTrained.csproj \
  dressermod=DresserMod/DresserMod.csproj \
  onlypockettedspecialammo=OnlyPockettedSpecialAmmo/OnlyPockettedSpecialAmmo.csproj \
  smallerhud=SmallerHUD/SmallerHUD.csproj

include Scripts/Makefile.common
