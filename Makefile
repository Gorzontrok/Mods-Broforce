# Mods-Broforce Makefile

PROJECT_NAME := Mods-Broforce

PROJECTS := \
  skipintro=SkipIntro/SkipIntroMod.csproj \
  expendabros=ExpendablesBrosInGame/ExpendablesBrosInGame.csproj \
  filteredbros=FilteredBros/FilteredBrosMod.csproj \
  007-patch=007_Patch/DoubleBroSevenTrained.csproj \
  dressermod=DresserMod/DresserMod.csproj \
  autoenterpassword=AutoEnterPassword/AutoEnterPassword.csproj \
  onlypockettedspecialammo=OnlyPockettedSpecialAmmo/OnlyPockettedSpecialAmmo.csproj \
  searchlocalcampaignfaster=SearchLocalCampaignFaster/SearchLocalCampaignFaster.csproj \
  smallerhud=SmallerHUD/SmallerHUD.csproj \
  showavatarfacehugger=ShowAvatarFaceHugger/AvatarFaceHuggerMod.csproj

include Scripts/Makefile.common
