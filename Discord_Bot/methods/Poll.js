const { EmbedBuilder, Embed } = require("discord.js");
const axios = require("axios");

async function editEmbed(whichField, interaction) {
  const username = interaction.user.username;

  // Update the embed to add the username as the last value of the specified field
  const oldEmbed = interaction.message.embeds[0];

  // Make a copy of the specified field
  const updatedField = { ...oldEmbed.fields[whichField] };

  var multipleChoiceValue;
  if(oldEmbed.footer.text == process.env.MultipleChoiceFalse) multipleChoiceValue = false
  else multipleChoiceValue = true;

  var isUsernameDisplayed = false;

  for (let i = 0; i < oldEmbed.fields.length; i++) {
    const field = oldEmbed.fields[i];

    if (field.value.includes(username)) {
      isUsernameDisplayed = true;
      break; // If username is found, no need to continue checking other fields
    }
  }

  if (multipleChoiceValue || !isUsernameDisplayed) {
    // Remove the default "-" value from the specified field if it exists
    if (updatedField.value === "-") {
      updatedField.value = "";
    }

    // Check if the username is already present in the specified field
    if (updatedField.value.includes(username)) {
      // Return a message indicating that the user has already voted
      return "You already voted for this option!";
    }

    // Append the username to the value of the specified field
    updatedField.value += `\n${username}`;

    // Copy the rest of the fields from the old embed to the new embed
    const newEmbed = new EmbedBuilder(oldEmbed).spliceFields(
      whichField,
      1,
      updatedField
    ); // Replace the specified field with the updated one

    // Add the remaining fields from the old embed to the new embed
    for (let i = 1; i < oldEmbed.fields.length; i++) {
      const field = oldEmbed.fields[i];
    }

    return newEmbed;
  } 

  return 'You already voted for this poll';
}

module.exports = {
  editEmbed: editEmbed,
};
