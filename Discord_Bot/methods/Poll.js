const {  EmbedBuilder, Embed } = require('discord.js');

function editEmbed(whichField, interaction) {
  const username = interaction.user.username;

  // Update the embed to add the username as the last value of the first field
  const oldEmbed = interaction.message.embeds[0];

  // Make a copy of the first field
  const updatedFirstField = { ...oldEmbed.fields[whichField] };

  // Remove the default "-" value from the first field if it exists
  if (updatedFirstField.value === '-') {
    updatedFirstField.value = '';
  }

  // Append the username to the value of the first field
  updatedFirstField.value += `\n${username}`;

  // Copy the rest of the fields from the old embed to the new embed
  const newEmbed = new EmbedBuilder(oldEmbed)
    .spliceFields(whichField, 1, updatedFirstField); // Replace the first field with the updated one

  // Add the remaining fields from the old embed to the new embed
  for (let i = 1; i < oldEmbed.fields.length; i++) {
    const field = oldEmbed.fields[i];
  }
    return newEmbed;
}

module.exports = {
  editEmbed: editEmbed,
};
